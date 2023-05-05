using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.Configuration;
using Sitecore.XConnect.Collection.Model;
using Feature.FormsExtensions.Models;

namespace Feature.FormsExtensions.SubmitActions
{
    public class SaveToContactList : SubmitActionBase<ContactListParameters>
    {
        public SaveToContactList(ISubmitActionData submitActionData) : base(submitActionData)
        {
        }

        protected override bool Execute(ContactListParameters data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(data, nameof(data));
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));

            var firstNameField = formSubmitContext.Fields.FirstOrDefault(field => field.Name.Equals("FirstName"));
            var lastNameField = formSubmitContext.Fields.FirstOrDefault(field => field.Name.Equals("LastName")); ;
            var emailField = formSubmitContext.Fields.FirstOrDefault(field => field.Name.Equals("Email"));

            if (firstNameField == null && lastNameField == null && emailField == null)
            {
                return false;
            }

            try
            {
                using (var client = CreateClient())
                {
                    return SaveContactInListManager(client, firstNameField, lastNameField, emailField, data.ReferenceId);
                }
            }
            catch (XdbExecutionException exception)
            {
                Logger.LogError(exception.Message, exception);
                return false;
            }
        }

        private bool SaveContactInListManager(IXdbContext client, IViewModel firstNameField, IViewModel lastNameField, IViewModel emailField, Guid contactListId)
        {
            try
            {
                var reference = new IdentifiedContactReference("ListManager", GetValue(emailField));

                var expandOptions = new ContactExpandOptions(
                        CollectionModel.FacetKeys.PersonalInformation,
                        CollectionModel.FacetKeys.EmailAddressList,
                        CollectionModel.FacetKeys.ListSubscriptions);

                var executionOptions = new ContactExecutionOptions(expandOptions);

                Contact contact = client.Get<Contact>(reference, executionOptions);

                if (contact == null)
                {
                    contact = new Sitecore.XConnect.Contact();
                    client.AddContact(contact);
                }

                SetPersonalInformation(GetValue(firstNameField), GetValue(lastNameField), contact, client);
                SetEmail(GetValue(emailField), contact, client);
                SetSubscriptionList(client, contact, contactListId);

                var contactIdentifier = new ContactIdentifier("ListManager", GetValue(emailField), ContactIdentifierType.Known);
                if (!contact.Identifiers.Contains(contactIdentifier))
                {
                    client.AddContactIdentifier(contact, contactIdentifier);
                }

                client.Submit();
                return true;
            }
            catch (XdbExecutionException exception)
            {
                Logger.LogError(exception.Message, exception);
                return false;
            }
        }

        private static void SetPersonalInformation(string firstName, string lastName, Contact contact, IXdbContext client)
        {
            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            {
                return;
            }

            PersonalInformation personalInfoFacet = contact.Personal() ?? new PersonalInformation();
            if (personalInfoFacet.FirstName == firstName && personalInfoFacet.LastName == lastName)
            {
                return;
            }

            personalInfoFacet.FirstName = firstName;
            personalInfoFacet.LastName = lastName;

            client.SetPersonal(contact, personalInfoFacet);
        }

        private static void SetEmail(string email, Contact contact, IXdbContext client)
        {
            if (string.IsNullOrEmpty(email))
            {
                return;
            }

            EmailAddressList emailFacet = contact.Emails();
            if (emailFacet == null)
            {
                emailFacet = new EmailAddressList(new EmailAddress(email, false), "Preferred");
            }
            else
            {
                if (emailFacet.PreferredEmail?.SmtpAddress == email)
                {
                    return;
                }

                emailFacet.PreferredEmail = new EmailAddress(email, false);
            }

            client.SetEmails(contact, emailFacet);
        }

        private void SetSubscriptionList(IXdbContext client, Contact contact, Guid contactListId)
        {
            var subscriptions = contact.ListSubscriptions();
            
            var listSubscription = subscriptions?.Subscriptions.FirstOrDefault(sub => sub.ListDefinitionId == contactListId && sub.IsActive == true);

            if (listSubscription != null)
            {
                return;
            }

            if (subscriptions == null)
            {
                subscriptions = new ListSubscriptions();
            }

            var listId = contactListId;
            var isActive = true;
            var added = DateTime.UtcNow;

            ContactListSubscription subscription = new ContactListSubscription(added, isActive, listId);

            subscriptions.Subscriptions.Add(subscription);
            client.SetListSubscriptions(contact, subscriptions);
        }

        protected virtual IXdbContext CreateClient()
        {
            return SitecoreXConnectClientConfiguration.GetClient();
        }

        private static string GetValue(object field)
        {
            return field?.GetType().GetProperty("Value")?.GetValue(field, null)?.ToString() ?? string.Empty;
        }

        private static IViewModel GetFieldById(Guid id, IList<IViewModel> fields)
        {
            return fields.FirstOrDefault(field => Guid.Parse(field.ItemId) == id);
        }
    }
}

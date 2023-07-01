# Forms-SaveContactsToListManager
Custom Submit Action for Sitecore Forms to Save Contacts in Sitecore List Manager's Contact List - Sitecore 10.2.

## PreRequisites
1. Sitecore 10.2 XP with SXA and List Manager installed in local
2. Developer workstation to validate the solution

## Setup
1. Take backup of webroot\bin and webroot\sitecore\shell folders.
2. Clone the repository Forms-SaveContactsToListManager (https://github.com/ravindra-mishra/Forms-SaveContactsToListManager)
3. Build the solution
4. Copy the Feature.FormsExtensions.dll to your webroot\bin
5. Follow instructions given on the blog to [create SPEAK editor](https://blogs.perficient.com/2023/06/01/submit-action-to-save-contacts-in-list-manager-with-fields-mapping-part-1/#create-speak-editor) OR Install the sitecore package [Core-SaveToContactList-SPEAK-Editor](./SitecorePackages/Core-SaveToContactList-SPEAK-Editor.zip).
6. Copy [SaveToContactList.js](https://github.com/ravindra-mishra/Forms-SaveContactsToListManager/blob/master/Feature.FormsExtensions/sitecore/shell/client/Applications/FormsBuilder/Layouts/Actions/SaveToContactList.js) file to  webroot/sitecore/shell/client/Applications/FormsBuilder/Layouts/Actions path (If you are installing sitecore package you can skip this step as it is already included in package.)
7. Follow instructions given on [Step 2: Create submit action item in master database of the blog](https://blogs.perficient.com/2023/06/02/submit-action-to-save-contacts-in-list-manager-with-fields-mapping-part-2/#submitActionClassAndItem) OR Install the sitecore package [Master-SaveToContactList-SubmitAction](https://github.com/ravindra-mishra/Forms-SaveContactsToListManager/blob/master/SitecorePackages/Master-SaveToContactList-SubmitAction.zip)
8. Login to Sitecore and follow the steps as below
9. Create an empty contact list called "Customer Contact List" in Lanchpad > List Manager > Create > Empty Contact List.
10. Make a sitecore form with the fields First Name, Last Name, and Email.
11. Add a Submit button and link it to the "Save To Contact List" Submit Action.
12. Map the form fields for first name, last name and email.
13. Save the form after selecting the newly formed Contact list.
14. Save the page item after adding the sitecore form.
15. Browse the web page and fill out the form's required information.
16. Check the data that has been provided (Lanchpad > List Manager > Contact List > "Customer Contact List" > Scroll below to Contacts selection)

# Exploring More Sitecore Insights
## Perficient Blog Post 
https://blogs.perficient.com/author/rmishra/
1. Add PowerShell Script to the Context Menu in Sitecore SXA - https://blogs.perficient.com/2022/09/30/add-powershell-script-to-the-context-menu-in-sitecore-sxa/
2. Submit Action to Save Contacts in List Manager – Basic Implementation - https://blogs.perficient.com/2023/05/13/submit-action-to-save-contacts-in-list-manager-basic-implementation/
3. Submit Action to Save Contacts in List Manager with Fields Mapping - https://blogs.perficient.com/2023/06/01/submit-action-to-save-contacts-in-list-manager-with-fields-mapping-part-1/

## Personal Sitecore Blog
https://dailysitecore.blogspot.com/

1. A Custom Switch Link Provider for Multisite setup in Sitecore 9.1 -
  https://dailysitecore.blogspot.com/2022/06/a-custom-switch-link-provider-for.html
2. Rewrite rule to convert URLs to small letters and removing spaces -
  https://dailysitecore.blogspot.com/2022/06/rewrite-rule-to-convert-urls-to-small.html
3. Get droplink value of rendering parameter using scriban in Sitecore - 
  https://dailysitecore.blogspot.com/2022/08/get-droplink-value-of-rendering-parameter-using-scriban.html

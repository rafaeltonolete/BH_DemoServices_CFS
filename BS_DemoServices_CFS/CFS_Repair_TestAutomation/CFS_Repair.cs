using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using CFS_Automation;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Linq;
using System.Collections.Generic;

namespace BH_DemoServices_CFS
{
    [TestClass]
    public class CFS_Repair
    {


        private readonly SecureString _username0 = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername0"].ToSecureString();
        private readonly SecureString _username1 = System.Configuration.ConfigurationManager.AppSettings["OnlineUsername1"].ToSecureString();
        private readonly SecureString _password0 = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword0"].ToSecureString();
        private readonly SecureString _password1 = System.Configuration.ConfigurationManager.AppSettings["OnlinePassword1"].ToSecureString();
        private readonly Uri _xrmUri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineCrmUrl"].ToString());




        [TestMethod]
        public void IoTDeviceSetUp()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                //****Variable declaration
                string iotCentralInstanceName = "A. Datum MX Chip Heat Bed Sensor";
                string parentWindowhandle = client.Browser.Driver.CurrentWindowHandle;


                //Log In
                xrmApp.OnlineLogin.Login(_xrmUri, _username0, _password0);
                xrmApp.ThinkTime(5000);

                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);



             //CREATE DEVICE IN IOT CENTRAL as SYSTEM ADMIN

                /*Log in as System admin and go to https://apps.azureiotcentral.com/ */
                client.Browser.Driver.Url = "https://apps.azureiotcentral.com/myapps";
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                /*Select the CFS IoT Central XX (where XX is the Initials of the TSP)*/
                client.Browser.Driver.FindElement(By.XPath(".//*[@title='CFS2 IOT Central JB']")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                /*Select the Device Explorer Icon*/
                client.Browser.Driver.FindElement(By.XPath(".//a[@title='Create and manage devices']")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                /*Select MXChip Template*/
                client.Browser.Driver.FindElement(By.XPath(".//a[@title='MXChip (1.0.0)']")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                /*Click new and select the Simulated option in the drop down*/
                client.Browser.Driver.FindElement(By.XPath(".//button[@aria-describedby='action-button-new-tt']")).Click();
                xrmApp.ThinkTime(5000);
                client.Browser.Driver.FindElement(By.XPath(".//button[@role='menuitem'][text()='Simulated']")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);



                /*Change the name to A. Datum MX Chip Heat Bed Sensor*/
                client.Browser.Driver.FindElement(By.XPath(".//input[@aria-label='Instance Name']")).Click();
                client.Browser.Driver.FindElement(By.XPath(".//input[@aria-label='Instance Name']")).Clear();
                client.Browser.Driver.FindElement(By.XPath(".//input[@aria-label='Instance Name']")).SendKeys(iotCentralInstanceName);
                client.Browser.Driver.FindElement(By.XPath(".//span[text()='Device']")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                Console.WriteLine("The New Simulated Device is CREATED");

                /*Copy the auto-generated device ID in this Test Script for later reference Refer to the URL of the device*/


                string DeviceIDUrl = client.Browser.Driver.Url;
                string DeviceId = DeviceIDUrl.Substring(DeviceIDUrl.Length - 20, 7);
                DeviceId = DeviceId.Trim('/');
                Console.WriteLine("The New Device ID is: " + DeviceId);
                xrmApp.ThinkTime(5000);





                //CONNECT IOT CENTRAL DEVICE TO DYNAMICS 365 DEVICE
                /*Navigate to Dynamics365 https://cfsv2test.crm.dynamics.com as System Adminstrator*/

                client.Browser.Driver.Url = "https://cfsv2test.crm.dynamics.com/main.aspx?";
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);


                client.Browser.Driver.FindElement(By.XPath(".//button[@data-id='navbar-switch-app'][@aria-label='Switch to another app']")).Click();
                xrmApp.ThinkTime(5000);


                client.Browser.Driver.FindElement(By.XPath("(//span[@class='item-label'][contains(text(),'Connected Field Service')])[1]")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);


                /*Check the Dashboard and Ensure that Default Dashboard is Connected  Field Service Dashboard */

                client.Browser.Driver.FindElement(By.XPath(".//span[contains(.,'Dashboards')]")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                

                /*In Field Service app, navigate to Service > Connected Devices > Devices Select A. Datum - Heat Bed Sensor - MXChip and update the Device ID*/

                client.Browser.Driver.FindElement(By.XPath(".//span[contains(text(),'Devices')]")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);



                xrmApp.Grid.Search("A. Datum Heat Bed Sensor - MXChip");
                xrmApp.ThinkTime(5000);
                xrmApp.Grid.OpenRecord(0);
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);



                /*Save the Device record*/

                xrmApp.Entity.ClearValue("msdyn_deviceid");
                xrmApp.Entity.SetValue("msdyn_deviceid", DeviceId);
                xrmApp.ThinkTime(5000);
                xrmApp.Entity.Save();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                /*Go to Service > My Work > Customer Assets*/
                client.Browser.Driver.FindElement(By.XPath("//span[contains(.,'Customer Assets')]")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                /*Select A. Datum - Heat Bed Sensor*/
                xrmApp.Grid.Search("A. Datum - Heat Bed Sensor");
                xrmApp.ThinkTime(5000);
                xrmApp.Grid.OpenRecord(0);
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                xrmApp.Entity.ClearValue("msdyn_deviceid");
                xrmApp.Entity.SetValue("msdyn_deviceid", DeviceId);
                xrmApp.ThinkTime(5000);
                xrmApp.Entity.Save();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);


                /*Click on the Device Record (A. Datum Heat Bed Sensor - MXChip) under Connected Devices*/

                client.Browser.Driver.FindElement(By.XPath(".//h2[@title='Connected Devices'][contains(.,'Connected Devices')]")).Click();
                xrmApp.ThinkTime(5000);
                client.Browser.Driver.FindElement(By.XPath(".//label[contains(@data-id,'ConnectedDevices-ListItemLabel_record')][contains(text(),'A. Datum Heat Bed Sensor - MXChip')]")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);


                /*Click on the Device Record (A. Datum Heat Bed Sensor - MXChip)*/
                client.Browser.Driver.FindElement(By.XPath(".//label[contains(text(),'A. Datum Heat Bed Sensor - MXChip')]")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                /*Click on the 'IoT central' button in the Command bar*/
                client.Browser.Driver.FindElement(By.XPath(".//span[@aria-label='IoT Central'][contains(.,'IoT Central')]")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);



                //(ADD SCREENSHOT)






                List<string> handles = client.Browser.Driver.WindowHandles.ToList();
                foreach (var handle in handles)
                {
                    if (!(handle == parentWindowhandle))
                    {
                        client.Browser.Driver.SwitchTo().Window(handle);
                        client.Browser.Driver.WaitForPageToLoad(60);
                        break;
                    }

                }




                /*Check the temperature threshold value by clicking on Rules*/
                var elements = client.Browser.Driver.FindElements(By.TagName("a"));
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                foreach (var element in elements)
                {
                    string attribVal = element.GetAttribute("title");
                    if (attribVal == "Rules")
                    {
                        element.Click();
                        break;
                    }
                }

                /*On the list of Rules Temperature Threshold> Conditions, Note: Take note of the value*/
                client.Browser.Driver.FindElement(By.XPath(".//li/span[contains(text(),'Temperature threshold')]")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);


                string TempthreshHoldLinkText = client.Browser.Driver.FindElement(By.XPath(".//li/span[contains(text(),'Temperature threshold')]")).Text.Replace(" ", "");

                string TempThreshholdVal = TempthreshHoldLinkText.Substring(21);
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);


                Console.WriteLine("The Set Temperature Threshhold Value is: "+TempThreshholdVal);

                //(ADD SCREENSHOT)





                xrmApp.Dispose(); 
            }




        }


        [TestMethod]
        public void TurnOnMSFlow()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {
                //****Variable declaration

                string parentWindowhandle = client.Browser.Driver.CurrentWindowHandle;


                //Log In
                xrmApp.OnlineLogin.Login(_xrmUri, _username0, _password0);


                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                /* Turn on the MS Flow for Generating Alerts from IoT Central. This should fire Alerts and device readings from the Iot Central. Follow the steps below:
                1. Go to web.powerapps.com and navigate to the correct environment (either Connected Field Service 2.0 or the Default instance where you imported the Flow)
                2. Go to Solutions > Connected Field Service 2.0 > Look for Solution: CDS Create alerts when Temperature is > 70 and turn on the process.
                */

                client.Browser.Driver.Url = "https://make.powerapps.com";
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                client.Browser.Driver.FindElement(By.XPath(".//a[@title='Solutions']")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                client.Browser.Driver.FindElement(By.XPath(".//a[contains(text(),'Connected Field Service 2.0')]")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                client.Browser.Driver.FindElement(By.XPath(".//a[contains(text(),'Solution: CDS Create CFS alerts when Temperature is > 70')]")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                List<string> handles = client.Browser.Driver.WindowHandles.ToList();
                foreach (var handle in handles)
                {
                    if (!(handle == parentWindowhandle))
                    {
                        client.Browser.Driver.SwitchTo().Window(handle);
                        client.Browser.Driver.WaitForPageToLoad(60);
                        break;
                    }

                }

                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                client.Browser.Driver.FindElement(By.XPath(".//div[@id='content-container']/section/landing/div/react-app//div[contains(@class,'ms-Button-label')][contains(text(),'Turn on')]")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

 



                xrmApp.Dispose();


            }



        }







        [TestMethod]
        public void TurnOffMSFlow()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {


                //Log In
                xrmApp.OnlineLogin.Login(_xrmUri, _username0, _password0);
                xrmApp.ThinkTime(5000);


                string parentWindowhandle = client.Browser.Driver.CurrentWindowHandle;

                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);


                client.Browser.Driver.Url = "https://make.powerapps.com";
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                client.Browser.Driver.FindElement(By.XPath(".//a[@title='Solutions']")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                client.Browser.Driver.FindElement(By.XPath(".//a[contains(text(),'Connected Field Service 2.0')]")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                client.Browser.Driver.WaitUntilVisible(By.XPath(".//a[contains(text(),'Solution: CDS Create CFS alerts when Temperature is > 70')]"));
                client.Browser.Driver.FindElement(By.XPath(".//a[contains(text(),'Solution: CDS Create CFS alerts when Temperature is > 70')]")).Click();
                client.Browser.Driver.WaitForPageToLoad();


                List<string> handles = client.Browser.Driver.WindowHandles.ToList();
                foreach (var handle in handles)
                {
                    if (!(handle == parentWindowhandle))
                    {
                        client.Browser.Driver.SwitchTo().Window(handle);
                        client.Browser.Driver.WaitForPageToLoad(60);
                        break;
                    }

                }

                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                client.Browser.Driver.FindElement(By.XPath(".//button[@role='menuitem'][@name='Turn off']")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);



                xrmApp.Dispose();


            }


        }



        [TestMethod]
        public void GregWinston()
        {
            var client = new WebClient(TestSettings.Options);
            using (var xrmApp = new XrmApp(client))
            {

                //Log In
                xrmApp.OnlineLogin.Login(_xrmUri, _username1, _password1);
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);


                string parentWindowhandle = client.Browser.Driver.CurrentWindowHandle;

                //Go to Field Service app
                client.Browser.Driver.SwitchTo().Frame("AppLandingPage");
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);
                client.Browser.Driver.FindElement(By.XPath(".//div[@title='Field Service']")).Click();
                
                xrmApp.ThinkTime(10000);
                client.Browser.Driver.SwitchTo().DefaultContent();


                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);
                

                client.Browser.Driver.FindElement(By.XPath(".//span[contains(.,'Dashboards')]")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                //Dashboard default

                string expectedDashboardName = "CFS Customer Service Representative Dashboard";
                string DisplayeddashBoardDefaultName = client.Browser.Driver.FindElement(By.XPath(".//span[contains(@id,'_text-value')]")).Text;
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);
                Assert.AreEqual(DisplayeddashBoardDefaultName, expectedDashboardName);
                Console.WriteLine("The expected Dashboard is correct.");

                //Go to Customers > Accounts 
                client.Browser.Driver.FindElement(By.XPath(".//li[@id='sitemap-entity-msdyn_AccountSubArea'][@title='Accounts']")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                //In the Active Accounts view, select A. Datum Corporation
                xrmApp.Grid.Search("A. Datum Corporation");
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);
                xrmApp.Grid.OpenRecord(0);
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                string acctNameExpected = "A. Datum Corporation";
                string acctNameDisplayed = xrmApp.Entity.GetValue("name");

                Assert.AreEqual(acctNameDisplayed, acctNameExpected);
                Console.WriteLine("Correct Account record is opened.");
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                //Check the Client 360 settings in the Account record

                client.Browser.Driver.SwitchTo().Frame("WebResource_Client360");

                bool casesWebResource360 = false;
                bool opportunitiesWebResource360 = false;
                bool woWebResource360 = false;


                if (client.Browser.Driver.FindElement(By.XPath(".//span[contains(text(),'CASES')]")).Displayed)
                {
                    client.Browser.Driver.FindElement(By.XPath(".//span[contains(text(),'CASES')]")).Click();
                    casesWebResource360 = true;
                }
                Assert.IsTrue(casesWebResource360);

                if (client.Browser.Driver.FindElement(By.XPath(".//span[contains(text(),'OPPORTUNITIES')]")).Displayed)
                {
                    client.Browser.Driver.FindElement(By.XPath(".//span[contains(text(),'OPPORTUNITIES')]")).Click();
                    opportunitiesWebResource360 = true;
                }
                Assert.IsTrue(opportunitiesWebResource360);
                if (client.Browser.Driver.FindElement(By.XPath(".//span[contains(text(),'WORK ORDERS')]")).Displayed)
                {
                    client.Browser.Driver.FindElement(By.XPath(".//span[contains(text(),'WORK ORDERS')]")).Click();
                    woWebResource360 = true;
                }
                Assert.IsTrue(woWebResource360);


                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                Console.WriteLine("The following tabs are present: Cases/Opportunities/Work Orders");


                //Go to the Timeline Pane of the Account record
                client.Browser.Driver.SwitchTo().DefaultContent();
                xrmApp.ThinkTime(10000);

                try
                {
                    client.Browser.Driver.FindElement(By.Id("TL_LoadMoreRecords")).Click();
                }
                catch(Exception e)
                {
                    throw e;
                }

                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                //Check that an email was sent  in the timeline of Customer regarding the Device Alert

                if (client.Browser.Driver.FindElement(By.XPath("(//div[@role='link'][contains(text(),'Device Anomaly encountered : A. Datum Heat Bed Sensor - MXChip')])[1]")).Displayed)
                {
                    Console.WriteLine("Device Alert Email is received");
                    client.Browser.Driver.FindElement(By.XPath("(//div[@role='link'][contains(text(),'Device Anomaly encountered : A. Datum Heat Bed Sensor - MXChip')])[1]")).Click();
                }

                string caseID = xrmApp.Entity.GetValue("ticketnumber");
                

                //Go to Service > Cases
                client.Browser.Driver.FindElement(By.XPath(".//span[contains(.,'Cases')]")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                xrmApp.Grid.SwitchView("Active Cases");
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);


                //Search for the latest case linked to 'A. Datum Heat Bed Sensor - MXChip' device(Case title: Device Anomaly encountered: A.Datum Heat Bed Sensor - MXChip)
                xrmApp.Grid.Search(caseID);
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);
                xrmApp.Grid.OpenRecord(0);
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                Console.WriteLine("Case ID: " + caseID);


                //Click on the 'Created' Stage to check the IoT Alert associated with the case Check that the following fields are populated: Device / Device ID

                client.Browser.Driver.FindElement(By.XPath(".//label[contains(text(),'Created')]")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                xrmApp.BusinessProcessFlow.SelectStage("Created");
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                if (client.Browser.Driver.FindElement(By.XPath(".//label[contains(@id,'header_process_msdyn_device')][contains(text(),'A. Datum Heat Bed Sensor - MXChip')]")).Displayed)
                {
                    Console.WriteLine("Correct Device name on Case");
                }
                else
                {
                    Assert.Fail();
                }

                //Go back to the Case page In the Case timeline, create a new Task Enter the information
                client.Browser.Driver.FindElement(By.XPath(".//span[contains(.,'Cases')]")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                xrmApp.Grid.SwitchView("Active Cases");
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);
                xrmApp.Grid.Search(caseID);
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);
                xrmApp.Grid.OpenRecord(0);
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);


                client.Browser.Driver.FindElement(By.XPath(".//label[contains(@data-id,'notescontrol-action_bar_add_command_id')]")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);
                client.Browser.Driver.FindElement(By.XPath(".//label[contains(text(),'Task')]")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);


                //input[@data-id='subject.fieldControl-text-box-text']
                /*QuickCreate Task Subject*/
                string subject = "Work Order and Resource scheduling request";
                client.Browser.Driver.FindElement(By.XPath(".//input[@data-id='subject.fieldControl-text-box-text']")).Click();
                client.Browser.Driver.FindElement(By.XPath(".//input[@data-id='subject.fieldControl-text-box-text']")).SendKeys(subject);
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                /*QuickCreate Task Description*/
                String myText = "Hi Spencer!\n Please create a Service Call WO for  A.Datum Heat Bed Sensor - MXChip Device\n and assign a field service agent for A.Datum Corporation";
                myText = myText.Replace("\n", Environment.NewLine);
                client.Browser.Driver.FindElement(By.XPath(".//div[@aria-label='Create Task']//textarea[contains(@aria-label,'Description')]")).Click();
                client.Browser.Driver.FindElement(By.XPath(".//div[@aria-label='Create Task']//textarea[contains(@aria-label,'Description')]")).SendKeys(myText);
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);
                /*QuickCreate Task Due date */
                string currentDate = (DateTime.Now).ToString("MM/dd/yyyy");
                client.Browser.Driver.FindElement(By.XPath(".//input[contains(@id,'DatePicker')][@aria-label='Due']")).SendKeys(currentDate);
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);
                /*QuickCreate Save and Close */
                client.Browser.Driver.FindElement(By.XPath(".//span[contains(text(),'Save and Close')]")).Click();
                client.Browser.Driver.WaitForPageToLoad();
                xrmApp.ThinkTime(5000);

                if (client.Browser.Driver.FindElement(By.XPath(".//label/div[contains(.,'"+ subject + "')]")).Displayed)
                {
                    Console.WriteLine("Task is successfully created");
                }

                //label/div[contains(.,'Test')]

                xrmApp.Dispose();
            }

        }

       

        //[TestMethod]
        //public void CodeTester()
        //{

         //   string currentDate = (DateTime.Now).ToString("MM/dd/yyyy");
            //    /*THIS WILL BE REMOVED WHEN SOLUTION IS FINALIZED*/
            //    /*THE PURPOSE FOR THIS BLOCK IS to SIMULATE CODE DURING DEVELOPMENT */
            //    var client = new WebClient(TestSettings.Options);
            //    using (var xrmApp = new XrmApp(client))
            


        //        //Log In
        //        xrmApp.OnlineLogin.Login(_xrmUri, _username0, _password0);
        //        xrmApp.ThinkTime(5000);

        //        client.Browser.Driver.WaitForPageToLoad();
        //        xrmApp.ThinkTime(5000);


        //        client.Browser.Driver.Url = "https://cfs2-iot-central-jb.azureiotcentral.com/details/device/1padhee/measurements";



        //        string DeviceIDUrl = client.Browser.Driver.Url;
        //        string DeviceId = DeviceIDUrl.Substring(DeviceIDUrl.Length - 20, 7);



        //        /*Check the temperature threshold value by clicking on Rules*/
        //        client.Browser.Driver.WaitForPageToLoad();
        //        xrmApp.ThinkTime(5000);

        //        var elements = client.Browser.Driver.FindElements(By.TagName("a"));

        //        client.Browser.Driver.WaitForPageToLoad();
        //        xrmApp.ThinkTime(5000);

        //        foreach (var element in elements)
        //        {

        //            string attribVal = element.GetAttribute("title");
        //            if (attribVal == "Rules")
        //            {


        //                element.Click();



        //                break;
        //            }

        //        }






        //        //client.Browser.Driver.FindElement(By.XPath("(//span[text()='Rules'])[1]")).Click();
        //        client.Browser.Driver.WaitForPageToLoad();
        //        xrmApp.ThinkTime(5000);

        //        /*On the list of Rules Temperature Threshold> Conditions, Note: Take note of the value*/
        //        client.Browser.Driver.FindElement(By.XPath(".//li/span[contains(text(),'Temperature threshold')]")).Click();
        //        client.Browser.Driver.WaitForPageToLoad();
        //        xrmApp.ThinkTime(5000);

        //        string TempthreshHoldLinkText = client.Browser.Driver.FindElement(By.XPath(".//li/span[contains(text(),'Temperature threshold')]")).Text.Replace(" ","");

        //        string TempThreshholdVal = TempthreshHoldLinkText.Substring(21);
        //        xrmApp.ThinkTime(5000);


        //        Console.WriteLine(TempThreshholdVal);


        //        xrmApp.ThinkTime(5000);

        //        ///*Select the CFS IoT Central XX (where XX is the Initials of the TSP)*/
        //        //client.Browser.Driver.FindElement(By.XPath(".//*[@class='inline-text-overflow'][@title='CFS2 IOT Central JB']")).Click();
        //        //client.Browser.Driver.WaitForPageToLoad();
        //        //xrmApp.ThinkTime(5000);

        //        ///*Select the Device Explorer Icon*/
        //        //client.Browser.Driver.FindElement(By.XPath(".//a[@title='Create and manage devices']")).Click();
        //        //client.Browser.Driver.WaitForPageToLoad();
        //        //xrmApp.ThinkTime(5000);

        //        ///*Select MXChip Template*/
        //        //client.Browser.Driver.FindElement(By.XPath(".//a[@title='MXChip (1.0.0)']")).Click();
        //        //client.Browser.Driver.WaitForPageToLoad();
        //        //xrmApp.ThinkTime(5000);


        //        //client.Browser.Driver.FindElement(By.XPath(".//a[contains(@class,'link')][contains(text(),'A. Datum MX Chip Heat Bed Sensor')]")).Click();
        //        //client.Browser.Driver.WaitForPageToLoad();
        //        //xrmApp.ThinkTime(5000);



        //        /////*https://cfs2-iot-central-jb.azureiotcentral.com/details/device/1padhee/measurements */
        //        ////string site = "https://cfs2-iot-central-jb.azureiotcentral.com/details/device/1padhee/measurements";
        //        //////int sitelen = site.Length;
        //        //////Console.WriteLine(sitelen);
        //        ////string DeviceId = site.Substring(site.Length - 20,7 );
        //        ////Console.WriteLine(DeviceId);

        //        //string DeviceIDUrl = client.Browser.Driver.Url;
        //        //string DeviceId = DeviceIDUrl.Substring(DeviceIDUrl.Length - 20, 7);
        //        //xrmApp.ThinkTime(5000);

        //        //Console.WriteLine(DeviceId);
        //        //xrmApp.ThinkTime(5000);





        //        xrmApp.Dispose();
         //   }




        //}






    }



}

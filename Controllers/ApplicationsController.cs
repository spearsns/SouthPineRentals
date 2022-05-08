using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using System.IO;
using LandlordV3_MVC;

namespace LandlordV3_MVC.Controllers
{
    public class ApplicationsController : Controller
    {
        private LandlordV3Entities db = new LandlordV3Entities();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Association(string appType)
        {
            Session["AppType"] = appType;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Association([Bind(Include = "Email")] PersonalData association)
        {
            if (ModelState.IsValid)
            {
                var obj1 = db.PersonalDatas.Where(a => a.Email.Equals(association.ID)).FirstOrDefault();
                if (obj1 != null)
                {
                    var obj2 = db.ApplicationDatas.Where(b => b.PersonalID.Equals(obj1.ID)).FirstOrDefault();
                    if (obj2 != null)
                    {
                        Session["AssociationID"] = obj2.ID.ToString();
                        return RedirectToAction("GeneralInfo", "Applications", new { appType = Session["AppType"].ToString() });
                    }// obj2 == null
                }// obj1 == null
            }// INVALID MODELSTATE
            return View(association);
        }

        public ActionResult GeneralInfo(string appType)
        {
            Session["AppType"] = appType;

            var property = new Property_Address();
            var locations = db.AddressDatas.Where(a => a.ID.Equals(property.AddressID));

            // Jareds Help
            foreach (AddressData location in locations)
            {
                SelectListItem listItem = new SelectListItem();
                listItem.Value = location.ID.ToString();
                listItem.Text = location.Address + " apt " + location.Unit;

                ViewBag.ListItems.Add(listItem);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GeneralInfo([Bind(Include = "FirstName,MiddleName,LastName,Suffix,DOB,Sex,Phone,Email")] PersonalData contactInfo)
        {
            if (ModelState.IsValid)
            {
                var personalData = new PersonalData
                {
                    ID = Guid.NewGuid(),
                    FirstName = contactInfo.FirstName,
                    MiddleName = contactInfo.MiddleName,
                    LastName = contactInfo.LastName,
                    Suffix = contactInfo.Suffix,
                    DOB = contactInfo.DOB,
                    Sex = contactInfo.Sex,
                    Phone = contactInfo.Phone,
                    Email = contactInfo.Email,
                    EntryTS = DateTime.Now
                };

                Session["PersonalID"] = personalData.ID;
                Session["Email"] = personalData.Email;

                db.PersonalDatas.Add(personalData);
                db.SaveChanges();
                return RedirectToAction("UploadApplication", "Applications");
            }
            return View(contactInfo);
        }


        /*
        public ActionResult UploadApplication()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadApplication([Bind(Include = "propertySelect")] UploadApplicationModel input, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                string location = input.ToString();
                Guid propertyAddressID;

                if (location.Contains(" apt "))
                {
                    int point = location.IndexOf(" apt ");
                    string address = location.Substring(0, point);
                    string apt = location.Substring(point);
                    var obj1 = db.AddressDatas.Where(a => a.Address.Equals(address) && a.Unit.Equals(apt)).FirstOrDefault();
                    var obj2 = db.Property_Address.Where(a => a.AddressID.Equals(obj1.ID)).FirstOrDefault();
                    propertyAddressID = obj2.ID;
                }
                else
                {
                    var obj1 = db.AddressDatas.Where(a => a.Address.Equals(location)).FirstOrDefault();
                    var obj2 = db.Property_Address.Where(a => a.AddressID.Equals(obj1.ID)).FirstOrDefault();
                    propertyAddressID = obj2.ID;
                }

                string folder;

                switch (location)
                {
                    case "312 S Pine St apt 1":
                        folder = "Content/Properties/312_S_Pine-1/Applications/";
                        break;
                    case "312 S Pine St apt 2":
                        folder = "Content/Properties/312_S_Pine-2/Applications/";
                        break;
                    case "528 S Pine St apt A":
                        folder = "Content/Properties/528_S_Pine-A/Applications/";
                        break;
                    case "528 S Pine St apt B":
                        folder = "Content/Properties/528_S_Pine-B/Applications/";
                        break;
                    case "528 S Pine St apt C":
                        folder = "Content/Properties/528_S_Pine-C/Applications/";
                        break;
                    case "601 W 21st St":
                        folder = "Content/Properties/601_W_21st/Applications/";
                        break;
                    case "603 W 21st St":
                        folder = "Content/Properties/603_W_21st/Applications/";
                        break;
                    case "1422 Porter St apt A":
                        folder = "Content/Properties/1422_Porter-A/Applications/";
                        break;
                    case "1422 Porter St apt B":
                        folder = "Content/Properties/1422_Porter-B/Applications/";
                        break;
                    case "1509 Stockton St":
                        folder = "Content/Properties/1509_Stockton/Applications/";
                        break;
                    default:
                        folder = "Content/Mgmt/Applications/";
                        break;
                }

                string personalID = Session["PersonalID"].ToString();
                int n;
                string date = DateTime.Today.ToString();

                var applicationData = new ApplicationData
                {
                    ID = Guid.NewGuid(),
                    PropertyAddressID = propertyAddressID,
                    PersonalID = Guid.Parse(personalID),
                    EntryTS = DateTime.Now
                };

                foreach (HttpPostedFileBase file in files)
                {
                    if (file != null)
                    {
                        if (file == files.ElementAt(index: 0))
                        {
                            var InputFileName = date + "_" + personalID + "_App";
                            var ServerSavePath = Path.Combine(Server.MapPath("~/" + folder) + InputFileName);
                            file.SaveAs(ServerSavePath);
                            applicationData.FileLoc = ServerSavePath;
                        }
                        if (file == files.ElementAt(index: 1))
                        {
                            n = 1;
                            var InputFileName = date + "_" + personalID + "_BS" + n;
                            var ServerSavePath = Path.Combine(Server.MapPath("~/" + folder) + InputFileName);
                            file.SaveAs(ServerSavePath);
                            applicationData.FirstBankStatementLoc = ServerSavePath;
                        }
                        if (file == files.ElementAt(index: 2))
                        {
                            n = 2;
                            var InputFileName = date + "_" + personalID + "_BS" + n;
                            var ServerSavePath = Path.Combine(Server.MapPath("~/" + folder) + InputFileName);
                            file.SaveAs(ServerSavePath);
                            applicationData.SecondBankStatementLoc = ServerSavePath;
                        }
                        if (file == files.ElementAt(index: 3))
                        {
                            n = 3;
                            var InputFileName = date + "_" + personalID + "_BS" + n;
                            var ServerSavePath = Path.Combine(Server.MapPath("~/" + folder) + InputFileName);
                            file.SaveAs(ServerSavePath);
                            applicationData.ThirdBankStatementLoc = ServerSavePath;
                        }
                        if (file == files.ElementAt(index: 4))
                        {
                            n = 1;
                            var InputFileName = date + "_" + personalID + "_CS" + n;
                            var ServerSavePath = Path.Combine(Server.MapPath("~/" + folder) + InputFileName);
                            file.SaveAs(ServerSavePath);
                            applicationData.FirstCreditStatementLoc = ServerSavePath;
                        }
                        if (file == files.ElementAt(index: 5))
                        {
                            n = 2;
                            var InputFileName = date + "_" + personalID + "_CS" + n;
                            var ServerSavePath = Path.Combine(Server.MapPath("~/" + folder) + InputFileName);
                            file.SaveAs(ServerSavePath);
                            applicationData.SecondCreditStatementLoc = ServerSavePath;
                        }
                        if (file == files.ElementAt(index: 6))
                        {
                            n = 3;
                            var InputFileName = date + "_" + personalID + "_CS" + n;
                            var ServerSavePath = Path.Combine(Server.MapPath("~/" + folder) + InputFileName);
                            file.SaveAs(ServerSavePath);
                            applicationData.ThirdCreditStatementLoc = ServerSavePath;
                        }
                    }
                }
                db.ApplicationDatas.Add(applicationData);
                db.SaveChanges();
                Session.Clear();
                Session.Abandon();
                return RedirectToAction("Success", "Home");
            }
            return View(files);
        }

        /* --V2 Version
        // COSIGNER || JOINT - APPLICANT
        public ActionResult Association(string appType)
        {
            Session["AppType"] = appType;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Association([Bind(Include = "ID")] Applications association)
        {
            if (ModelState.IsValid)
            {
                var obj1 = db.Applications.Where(a => a.ID.Equals(association.ID)).FirstOrDefault();
                if (obj1 != null)
                {
                    Session["AssociationID"] = obj1.ID.ToString();
                    return RedirectToAction("PersonalInfo", "Applications", new { appType = Session["AppType"].ToString() });
                }// obj1 == null
            }// INVALID MODELSTATE
            return View(association);
        }

        // PERSONAL INFO
        public ActionResult PersonalInfo(string appType)
        {
            Session["AppType"] = appType;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersonalInfo([Bind(Include = "FirstName,LastName,DOB,Sex,Address,Unit,City,State,ZIP,Phone,Email")] Models.ContactInfoView contactInfo, bool exit = false)
        {
            if (ModelState.IsValid)
            {
                var personalData = new PersonalData
                {
                    ID = Guid.NewGuid(),
                    FirstName = contactInfo.FirstName,
                    LastName = contactInfo.LastName,
                    DOB = contactInfo.DOB,
                    Sex = contactInfo.Sex,
                    Phone = contactInfo.Phone,
                    Email = contactInfo.Email,
                    EntryTS = DateTime.Now
                };

                Session["PersonalID"] = personalData.ID;
                Session["Email"] = personalData.Email;
                
                var addressData = new AddressData
                {
                    ID = Guid.NewGuid(),
                    ReferenceID = personalData.ID,
                    Address = contactInfo.Address,
                    Unit = contactInfo.Unit,
                    City = contactInfo.City,
                    State = contactInfo.State,
                    ZIP = contactInfo.ZIP,
                    EntryTS = DateTime.Now 
                };

                db.PersonalData.Add(personalData);
                db.AddressData.Add(addressData);
                db.SaveChanges();
                if (exit == true) RedirectToAction("SaveAndExit");
                else return RedirectToAction("IncomeInfo", new { appType = Session["AppType"].ToString() });
            }
            return View(contactInfo);
        }

        // INCOME INFO (MANY : ONE)
        public ActionResult IncomeInfo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IncomeInfo([Bind(Include = "Employer,StartDate,EndDate,Position,Supervisor,Phone,Pay,PayPer,PayFrequency,AdditionalEarnings,AE_Frequency,GovtAssistance,GA_Frequency,CourtOrder,CO_Pay,CO_Frequency")] IncomeData incomeData, bool exit = false, bool addAnother = false)
        {
            if (ModelState.IsValid)
            {
                incomeData.ID = Guid.NewGuid();
                incomeData.PersonalID = Guid.Parse(Session["PersonalID"].ToString());
                incomeData.EntryTS = DateTime.Now;
                db.IncomeData.Add(incomeData);
                db.SaveChanges();

                if (addAnother == true) return RedirectToAction("IncomeInfo");
                else if (exit == true) return RedirectToAction("SaveAndExit");
                else return RedirectToAction("BankOption");
            }
            return View(incomeData);
        }
        // OPTION
        public ActionResult Option(string target)
        {
            ViewBag.target = target;
            return View();
        }

        // BANK INFO
        public ActionResult BankInfo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BankInfo([Bind(Include = "Institution,BranchOffice,AccountNumber,Balance")] BankData bankData, bool exit = false)
        {
            if (ModelState.IsValid)
            {
                bankData.ID = Guid.NewGuid();
                bankData.PersonalID = Guid.Parse(Session["PersonalID"].ToString());
                bankData.EntryTS = DateTime.Now;
                db.BankData.Add(bankData);
                db.SaveChanges();
                if (exit == true) return RedirectToAction("SaveAndExit");
                else return RedirectToAction("");
            }
            return View(bankData);
        }

        // BANK STATEMENTS
        public ActionResult BankStatements()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BankStatements(IEnumerable<HttpPostedFileBase> files)
        //(HttpPostedFileBase statementOne, HttpPostedFileBase statementTwo, HttpPostedFileBase statementThree)
        { 
            if (ModelState.IsValid)
            {   
                string date = DateTime.Now.ToString("MM-dd-yyyy");
                var personalID = Guid.Parse(Session["PersonalID"].ToString());
                int count = 1;

                foreach (var file in files)
                {
                    var inputFileName = date + "_" + personalID + "_" + count;
                    count++;
                    var serverSavePath = Path.Combine(Server.MapPath("~/Content/Statements/Bank/") + inputFileName);
                    var bankStatements = new BankStatements
                    {
                        ID = Guid.NewGuid(),
                        PersonalID = personalID,
                        FileLoc = inputFileName,
                        EntryTS = DateTime.Now
                    };
                    db.BankStatements.Add(bankStatements);
                    file.SaveAs(serverSavePath);
                }
                db.SaveChanges();
                return RedirectToAction("");
            }// ModelState Invalid
            return View(files);
        }

        // CREDIT INFO (MANY : ONE) -- OPTIONAL BYPASS
        public ActionResult CreditInfo()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreditInfo([Bind(Include = "Institution,AccountNumber,IssueDate,ExpirationDate,CreditLimit,Balance")] CreditData creditData, bool exit =  false, bool addAnother = false)
        {
            if (ModelState.IsValid)
            {
                creditData.ID = Guid.NewGuid();
                creditData.PersonalID = Guid.Parse(Session["PersonalID"].ToString());
                creditData.EntryTS = DateTime.Now;
                db.CreditData.Add(creditData);
                db.SaveChanges();
                if (addAnother == true) return RedirectToAction("CreditInfo");
                else if (exit == true) return RedirectToAction("SaveAndExit");
                else return RedirectToAction("CandidateInfo");
            }
            return View(creditData);
        }

        // CREDIT STATEMENTS
        public ActionResult CreditStatements()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreditStatements(IEnumerable<HttpPostedFileBase> files)
        //(HttpPostedFileBase statementOne, HttpPostedFileBase statementTwo, HttpPostedFileBase statementThree)
        {
            if (ModelState.IsValid)
            {
                string date = DateTime.Now.ToString("MM-dd-yyyy");
                var personalID = Guid.Parse(Session["PersonalID"].ToString());
                int count = 1;

                foreach (var file in files)
                {
                    var inputFileName = date + "_" + personalID + "_" + count;
                    count++;
                    var serverSavePath = Path.Combine(Server.MapPath("~/Content/Statements/Credit/") + inputFileName);
                    var creditStatements = new CreditStatements
                    {
                        ID = Guid.NewGuid(),
                        PersonalID = personalID,
                        FileLoc = inputFileName,
                        EntryTS = DateTime.Now
                    };
                    db.CreditStatements.Add(creditStatements);
                    file.SaveAs(serverSavePath);
                }
                db.SaveChanges();
                return RedirectToAction("");
            }// ModelState Invalid
            return View(files);
        }

        // CANDIDATE INFO -- END COSIGNER
        public ActionResult CandidateInfo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CandidateInfo([Bind(Include = "DriversLicense,DL_State,OwnRealEstate,ORE_Text,Evictions,E_Text,PaymentRefusal,PR_Text,DisruptiveCircumstances,DC_Text")] CandidateData candidateData, bool exit = false)
        {
            if (ModelState.IsValid)
            {
                candidateData.ID = Guid.NewGuid();
                candidateData.PersonalID = Guid.Parse(Session["PersonalID"].ToString());
                candidateData.EntryTS = DateTime.Now;

                if (Session["AppType"].ToString() == "Cosigner")
                {
                    Guid personalID = Guid.Parse(Session["PersonalID"].ToString());
                    Guid assocID = Guid.Parse(Session["AssociationID"].ToString());
                    Applications application = db.Applications.Find(assocID);
                    application.CosignerID = personalID;

                    var cosigner = new Cosigners
                    {
                        ID = Guid.NewGuid(),
                        ApplicationID = assocID,
                        PersonalID = personalID,
                        EntryTS = DateTime.Now
                    };

                    // ADD ALTERATION LOG LOGIC HERE
                    /*
                    var alteration = new AlterationLog
                    {
                        ID = Guid.NewGuid(),
                        TableName = Applications,
                        ColumnName = CosignerID,
                        UserID = ??? MAYBE FOR COSIGNERS / MAYBE NOT,
                        OldValue = ??? ASSUME NULL,
                        NewValue = personalID,
                        EntryTS = DateTime.Now
                    }
                    db.AlterationLog.Add(alteration);
                    
                    db.Entry(application).State = EntityState.Modified;
                    db.CandidateData.Add(candidateData);
                    db.Cosigners.Add(cosigner);
                    db.SaveChanges();
                    
                    return RedirectToAction("Success", "Home");
                }
                else if (exit == true) return RedirectToAction("SaveAndExit");
                else return RedirectToAction("RentalHistory");
            }
            return View(candidateData);
        }

        //RENTAL HISTORY
        public ActionResult RentalHistory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RentalHistory([Bind(Include = "Address,Unit,City,State,Zip,Rent,Landlord,Phone,Duration,Reason")] Models.RentalHistoryView rentalInfo, bool exit = false)
        {
            if (ModelState.IsValid)
            {
                var rentalHistory = new RentalHistory
                {
                    ID = Guid.NewGuid(),
                    CandidateID = Guid.Parse(Session["PersonalID"].ToString()),
                    Rent = rentalInfo.Rent,
                    Landlord = rentalInfo.Landlord,
                    Phone = rentalInfo.Phone,
                    Duration = rentalInfo.Duration,
                    Reason = rentalInfo.Reason,
                    EntryTS = DateTime.Now
                };

                var addressData = new AddressData
                {
                    ID = Guid.NewGuid(),
                    ReferenceID = rentalHistory.ID,
                    Address = rentalInfo.Address,
                    Unit = rentalInfo.Unit,
                    City = rentalInfo.City,
                    State = rentalInfo.State,
                    ZIP = rentalInfo.ZIP,
                    EntryTS = DateTime.Now
                };

                db.RentalHistory.Add(rentalHistory);
                db.AddressData.Add(addressData);
                db.SaveChanges();
                if (exit == true) return RedirectToAction("SaveAndExit");
                else return RedirectToAction("ReferenceInfo");
            }

            return View(rentalInfo);
        }

        // PERSONAL REFERENCES (MANY : ONE)
        public ActionResult ReferenceInfo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReferenceInfo([Bind(Include = "FirstName,LastName,Sex,Relationship,Address,Unit,City,State,ZIP,Phone,Email")] Models.ContactInfoView contactInfo, bool exit = false, bool addAnother = false)
        {
            if (ModelState.IsValid)
            {
                var personalData = new PersonalData
                {
                    ID = Guid.NewGuid(),
                    FirstName = contactInfo.FirstName,
                    LastName = contactInfo.LastName,
                    Sex = contactInfo.Sex,
                    Relationship = contactInfo.Relationship,
                    Phone = contactInfo.Phone,
                    Email = contactInfo.Email,
                    EntryTS = DateTime.Now
                };

                var addressData = new AddressData
                {
                    ID = Guid.NewGuid(),
                    Address = contactInfo.Address,
                    Unit = contactInfo.Unit,
                    City = contactInfo.City,
                    State =  contactInfo.State,
                    ZIP = contactInfo.ZIP,
                    EntryTS = DateTime.Now
                };

                var reference = new References
                {
                    ID = Guid.NewGuid(),
                    CandidateID = Guid.Parse(Session["PersonalID"].ToString()),
                    PersonalID = personalData.ID,
                    EntryTS = DateTime.Now
                };
                
                db.PersonalData.Add(personalData);
                db.AddressData.Add(addressData);
                db.References.Add(reference);
                db.SaveChanges();

                if (addAnother == true) return RedirectToAction("ReferenceInfo");
                else if (exit == true) return RedirectToAction("SaveAndExit");
                else return RedirectToAction("AdditionalResidents");
            }

            return View(contactInfo);
        }

        // ADDITIONAL RESIDENTS (MANY : ONE) -- OPTIONAL BYPASS
        public ActionResult AdditionalResidents()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdditionalResidents([Bind(Include = "FirstName,LastName,DOB,Sex,Phone,Email,Relationship")] PersonalData personalData, bool exit = false, bool addAnother = false)
        {
            if (ModelState.IsValid)
            {

                personalData.ID = Guid.NewGuid();
                personalData.EntryTS = DateTime.Now;
                
                var additionalResidents = new AdditionalResidents
                {
                    ID = Guid.NewGuid(),
                    CandidateID = Guid.Parse(Session["PersonalID"].ToString()),
                    PersonalID = personalData.ID,
                    EntryTS = DateTime.Now
                };
                
                db.PersonalData.Add(personalData);
                db.AdditionalResidents.Add(additionalResidents);
                db.SaveChanges();
                if (addAnother == true) return RedirectToAction("AdditionalResidents"); 
                else if (exit == true) return RedirectToAction("SaveAndExit");
                else return RedirectToAction("PetInfo");
            }
            return View(personalData);
        }

        // PET INFO (MANY : ONE) -- OPTIONAL BYPASS
        public ActionResult PetInfo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PetInfo([Bind(Include = "Type,Breed,Name,Weight,DOB,Description,Notes")] Pets pets, bool exit = false, bool addAnother = false)
        {
            if (ModelState.IsValid)
            {
                pets.ID = Guid.NewGuid();
                pets.CandidateID = Guid.Parse(Session["PersonalID"].ToString());
                pets.EntryTS = DateTime.Now;
                db.Pets.Add(pets);
                db.SaveChanges();
                if (addAnother == true) return RedirectToAction("PetInfo");
                else if (exit == true) return RedirectToAction("SaveAndExit");
                else return RedirectToAction("VehicleInfo");
            }
            return View(pets);
        }

        // VEHICLE INFO (MANY : ONE) -- OPTIONAL BYPASS
        public ActionResult VehicleInfo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VehicleInfo([Bind(Include = "Type,Year,Make,Model,Color,PlateNumber")] Vehicles vehicles, bool exit = false, bool addAnother = false)
        {
            if (ModelState.IsValid)
            {
                vehicles.ID = Guid.NewGuid();
                vehicles.CandidateID = Guid.Parse(Session["PersonalID"].ToString());
                vehicles.EntryTS = DateTime.Now;
                db.Vehicles.Add(vehicles);
                db.SaveChanges(); 
                if (exit == true) return RedirectToAction("SaveAndExit");
                else if (addAnother == true) return RedirectToAction("IncomeInfo");
                else return RedirectToAction("EmergencyContacts");
            }

            return View(vehicles);
        }

        // EMERGENCY CONTACTS (MANY : ONE)
        public ActionResult EmergencyContacts()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmergencyContacts([Bind(Include = "FirstName,LastName,Sex,Address,Apartment,City,State,ZIP,Phone,Email,Relationship")] Models.ContactInfoView contactInfo,bool addAnother = false)
        {
            if (ModelState.IsValid)
            {
                var personalData = new PersonalData
                {
                    ID = Guid.NewGuid(),
                    FirstName = contactInfo.FirstName,
                    LastName = contactInfo.LastName,
                    Sex = contactInfo.Sex,
                    Relationship = contactInfo.Relationship,
                    Phone = contactInfo.Phone,
                    Email = contactInfo.Email,
                    EntryTS = DateTime.Now
                };

                var addressData = new AddressData
                {
                    ID = Guid.NewGuid(),
                    Address = contactInfo.Address,
                    Unit = contactInfo.Unit,
                    City = contactInfo.City,
                    State = contactInfo.State,
                    ZIP = contactInfo.ZIP,
                    EntryTS = DateTime.Now
                };

                var emergencyContact = new EmergencyContacts
                {
                    ID = Guid.NewGuid(),
                    CandidateID = Guid.Parse(Session["PersonalID"].ToString()),
                    PersonalID = personalData.ID,
                    EntryTS = DateTime.Now
                };

                if (addAnother == true)
                {
                    db.PersonalData.Add(personalData);
                    db.EmergencyContacts.Add(emergencyContact);
                    return RedirectToAction("EmergencyContacts");
                }
                else // SUBMIT APPLICATION AND / OR PROPOSAL
                {
                    // CO-SIGNER ALWAYS REGISTERS AFTER INITIAL APPLICANT
                    var application = new Applications
                    {
                        ID = Guid.NewGuid(),
                        CandidateID = Guid.Parse(Session["PersonalID"].ToString()),
                        CosignerID = null,
                        EntryTS = DateTime.Now
                    };

                    // JOINT-APPLICATION LOGIC -- NOT CATCHING ASSOCIATE ID
                    if (Session["AppType"].ToString() == "Joint")
                    {
                        Guid assocID = Guid.Parse(Session["AssociationID"].ToString());
                        var jointApplication = new JointApplications
                        {
                            ID = Guid.NewGuid(),
                            OriginalAppID = assocID,
                            JointAppID = application.ID,
                            EntryTS = DateTime.Now
                        };

                        db.PersonalData.Add(personalData);
                        db.AddressData.Add(addressData);
                        db.EmergencyContacts.Add(emergencyContact);
                        db.Applications.Add(application);
                        db.JointApplications.Add(jointApplication);

                        db.SaveChanges(); 
                        return RedirectToAction("Success", "Home");
                    } // JOINT APPLICATION LOGIC

                    else // SINGLE APPLICATION LOGIC
                    {
                        var proposal = new Proposals
                        {
                            ID = Guid.NewGuid(),
                            PropertyID = null,
                            OriginalAppID = application.ID,
                            EntryTS = DateTime.Now
                        };

                        db.PersonalData.Add(personalData);
                        db.EmergencyContacts.Add(emergencyContact);
                        db.AddressData.Add(addressData);
                        db.Applications.Add(application);
                        db.Proposals.Add(proposal);
                        db.SaveChanges();

                        Session.Clear();
                        Session.Abandon();
                        return RedirectToAction("Success", "Home");
                    } // SINGLE APPLICATION LOGIC
                } // SUBMIT APPLICATION & PROPOSAL
            } // MODEL STATE VALID
            return View(contactInfo);
        }

        // SAVE AND EXIT
        // VALIDATION NOT INCLUDED!!!
        // SET CLIENT SIDE TO NOT BE AN OPTION UNTIL THEY FINISH FORM
        public ActionResult SaveAndExit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAndExit([Bind(Include = "Password")] Users users)
        {
            if (ModelState.IsValid)
            {
                var obj1 = db.Roles.Where(a => a.Name.Equals(Session["AppType"].ToString())).FirstOrDefault();
                if (obj1 != null)
                {
                    string roleID = obj1.ID.ToString();
                    users.ID = Guid.NewGuid();
                    users.RoleID = Guid.Parse(roleID);
                    users.Email = Session["Email"].ToString();
                    users.EntryTS = DateTime.Now;
                    users.LastLogin = DateTime.Now;
                    db.Users.Add(users);
                    
                    // JOINT OR CO-SIGN
                    if (Session["AssociateID"] != null && Session["UserID"] is null)
                    {
                        var association = new AssociationData
                        {
                            ID = Guid.NewGuid(),
                            ApplicationID = Guid.Parse(Session["AssociateID"].ToString()),
                            UserID = users.ID,
                            EntryTS = DateTime.Now
                        };

                        db.AssociationData.Add(association);
                    }// IF userID != null THEN ASSOCIATION ALREADY ESTABLISHED...

                    db.SaveChanges();
                    return RedirectToAction("Success", "Home");
                }// else (Roles.ID != AppType)...
            }// MODEL STATE INVALID

            return View(users);
        }
        
                    */
    }
}

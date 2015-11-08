using System;
using System.Linq;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeDifference.BusinessClasses;
using TimeDifference.Entity;
using TimeDifference.Services.Controllers;
using UserRole = TimeDifference.BusinessClasses.UserRole;

namespace TimeDifference.Tests
{
    [TestClass]
    public class TestTimeZoneEntryManagementController
    {

        private TransactionScope _trans;

        [TestInitialize]
        public void Initialize()
        {
            _trans = new TransactionScope(TransactionScopeOption.RequiresNew);



        }

        [TestCleanup]
        public void Cleanup()
        {
            new TimeDifference.Services.UserManager.UserInformation(null);
            _trans.Dispose();

        }


        #region GetUserTimeZoneInformation

        [TestMethod]
        public void GetUserTimeZoneInformation_ShouldGetUserTimeZoneInformationBasedOnIDPassed()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.Admin,
                UserName = "Test User"

            };
            var userId = new Data.UserMethods().RegisterUser(user);

            var otherUser = new RegistrationModel()
            {
                Email = "abc1@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };

            var otherUserId = new Data.UserMethods().RegisterUser(otherUser);

            #region AddingTimeZoneInformation

            var timeZoneId1 = new Data.TimeZoneEntryMethods().SaveNewTimeZoneEntry(new TimeZoneEntry { City = "City1", Difference = new TimeSpan(5, 30, 0).Ticks, EntryName = "Entry 1", IsActive = true, UserId = otherUserId });
            var timeZoneId2 = new Data.TimeZoneEntryMethods().SaveNewTimeZoneEntry(new TimeZoneEntry { City = "City2", Difference = new TimeSpan(5, 30, 0).Ticks, EntryName = "Entry 2", IsActive = true, UserId = otherUserId });
            var timeZoneId3 = new Data.TimeZoneEntryMethods().SaveNewTimeZoneEntry(new TimeZoneEntry { City = "City3", Difference = new TimeSpan(5, 30, 0).Ticks, EntryName = "Entry 3", IsActive = true, UserId = otherUserId });
            var timeZoneId4 = new Data.TimeZoneEntryMethods().SaveNewTimeZoneEntry(new TimeZoneEntry { City = "City4", Difference = new TimeSpan(5, 30, 0).Ticks, EntryName = "Entry 4", IsActive = true, UserId = otherUserId });

            #endregion



            new TimeDifference.Services.UserManager.UserInformation(new UserModel
            {
                Email = user.Email,
                Role = user.RoleId,
                UserId = userId,
                UserName = user.UserName
            });

            // Arrange
            var controller = new TimeZoneEntryManagementController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetUserTimeZoneInformation(otherUserId, 1, 100, 1, 100);

            // Assert
            Assert.AreEqual(response.Count, 1);
            Assert.IsTrue(response.Any());
            Assert.IsTrue(response.FirstOrDefault().TimeZoneEntries.Any());

            Assert.AreEqual(response.FirstOrDefault().UserId, otherUserId);
            Assert.AreEqual(response.FirstOrDefault().UserName, "Test User");
            Assert.AreEqual(response.FirstOrDefault().TimeZoneEntries.Count, 4);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Invalid ID Provided")]
        public void GetUserTimeZoneInformation_ShouldGetExceptionInvalidID()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.Admin,
                UserName = "Test User"

            };
            var userId = new Data.UserMethods().RegisterUser(user);



            new TimeDifference.Services.UserManager.UserInformation(new UserModel
            {
                Email = user.Email,
                Role = user.RoleId,
                UserId = userId,
                UserName = user.UserName
            });

            // Arrange
            var controller = new TimeZoneEntryManagementController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetUserTimeZoneInformation(-1, 1, 100, 1, 100);

            // Assert
            Assert.Fail("Invalid ID Provided");
        }

        #endregion

        #region GetTimeZoneInformation
        [TestMethod]
        public void GetTimeZoneInformation_ShouldGetTimeZoneInformationBasedOnIDPassed()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.Admin,
                UserName = "Test User"

            };
            var userId = new Data.UserMethods().RegisterUser(user);

            var otherUser = new RegistrationModel()
            {
                Email = "abc1@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };

            var otherUserId = new Data.UserMethods().RegisterUser(otherUser);

            #region AddingTimeZoneInformation

            var testData = new TimeZoneEntry
            {
                City = "City1",
                Difference = new TimeSpan(5, 30, 0).Ticks,
                EntryName = "Entry 1",
                IsActive = true,
                UserId = otherUserId
            };
            var timeZoneId1 = new Data.TimeZoneEntryMethods().SaveNewTimeZoneEntry(testData);

            #endregion



            new TimeDifference.Services.UserManager.UserInformation(new UserModel
            {
                Email = user.Email,
                Role = user.RoleId,
                UserId = userId,
                UserName = user.UserName
            });

            // Arrange
            var controller = new TimeZoneEntryManagementController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetTimeZoneInformation(timeZoneId1);

            // Assert
            Assert.AreEqual(response.City, testData.City);
            Assert.AreEqual(response.EntryName, testData.EntryName);
            Assert.AreEqual(response.UserId, testData.UserId);
            Assert.AreEqual(response.HourDifference, new TimeSpan(testData.Difference).Hours);
            Assert.AreEqual(response.MinuteDifference, new TimeSpan(testData.Difference).Minutes);

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Invalid ID Provided")]
        public void GetTimeZoneInformation_ShouldGetExceptionInvalidID()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.Admin,
                UserName = "Test User"

            };
            var userId = new Data.UserMethods().RegisterUser(user);



            new TimeDifference.Services.UserManager.UserInformation(new UserModel
            {
                Email = user.Email,
                Role = user.RoleId,
                UserId = userId,
                UserName = user.UserName
            });

            // Arrange
            var controller = new TimeZoneEntryManagementController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetTimeZoneInformation(-1);

            // Assert
            Assert.Fail("Invalid ID Provided");
        }
        #endregion

        #region SaveTimeZone
        [TestMethod]
        public void SaveTimeZone_ShouldSaveNewTimeZoneInformation()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.Admin,
                UserName = "Test User"

            };
            var userId = new Data.UserMethods().RegisterUser(user);

            var otherUser = new RegistrationModel()
            {
                Email = "abc1@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };

            var otherUserId = new Data.UserMethods().RegisterUser(otherUser);




            // var timeZoneId1 = new Data.TimeZoneEntryMethods().SaveNewTimeZoneEntry(testData);





            new TimeDifference.Services.UserManager.UserInformation(new UserModel
            {
                Email = user.Email,
                Role = user.RoleId,
                UserId = userId,
                UserName = user.UserName
            });

            // Arrange


            var controller = new TimeZoneEntryManagementController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act


            var testData = new TimeZoneEntryModel
            {
                City = "City1",
                MinuteDifference = 30,
                EntryName = "Entry 1",
                HourDifference = 5,
                UserId = otherUserId,
            };
            var savedTimeZoneId = controller.SaveTimeZone(testData);

            // Assert
            //Getting Saved Information From DataBase
            var timeZoneInformation = new Data.TimeZoneEntryMethods().GetTimeZoneEntry(savedTimeZoneId);

            Assert.IsTrue(timeZoneInformation != null);
            Assert.AreEqual(new TimeSpan(timeZoneInformation.Difference).Minutes, testData.MinuteDifference);
            Assert.AreEqual(new TimeSpan(timeZoneInformation.Difference).Hours, testData.HourDifference);

            Assert.AreEqual(timeZoneInformation.City, testData.City);
            Assert.AreEqual(timeZoneInformation.EntryName, testData.EntryName);
            Assert.AreEqual(timeZoneInformation.UserId, testData.UserId);

        }


        [TestMethod]
        public void SaveTimeZone_ShouldSaveExistingTimeZoneInformation()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.Admin,
                UserName = "Test User"

            };
            var userId = new Data.UserMethods().RegisterUser(user);

            var otherUser = new RegistrationModel()
            {
                Email = "abc1@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };

            var otherUserId = new Data.UserMethods().RegisterUser(otherUser);




            var timeZoneId1 = new Data.TimeZoneEntryMethods().SaveNewTimeZoneEntry(new TimeZoneEntry
            {

                City = "City1",
                EntryName = "Entry 1",
                UserId = otherUserId,
                Difference = new TimeSpan(5, 30, 0).Ticks,
                IsActive = true
            });





            new TimeDifference.Services.UserManager.UserInformation(new UserModel
            {
                Email = user.Email,
                Role = user.RoleId,
                UserId = userId,
                UserName = user.UserName
            });

            // Arrange


            var controller = new TimeZoneEntryManagementController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act


            var testData = new TimeZoneEntryModel
            {
                City = "City2",
                MinuteDifference = 50,
                EntryName = "Entry 2",
                HourDifference = 1,
                UserId = otherUserId,
                TimeZoneEntryId = timeZoneId1
            };
            var savedTimeZoneId = controller.SaveTimeZone(testData);

            // Assert
            //Getting Saved Information From DataBase
            var timeZoneInformation = new Data.TimeZoneEntryMethods().GetTimeZoneEntry(savedTimeZoneId);

            Assert.IsTrue(timeZoneInformation != null);
            Assert.AreEqual(new TimeSpan(timeZoneInformation.Difference).Minutes, testData.MinuteDifference);
            Assert.AreEqual(new TimeSpan(timeZoneInformation.Difference).Hours, testData.HourDifference);

            Assert.AreEqual(timeZoneInformation.City, testData.City);
            Assert.AreEqual(timeZoneInformation.EntryName, testData.EntryName);
            Assert.AreEqual(timeZoneInformation.UserId, testData.UserId);

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Invalid ID Provided")]
        public void SaveTimeZone_ShouldGetExceptionInvalidID_ModifyingExistingEntry()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.Admin,
                UserName = "Test User"

            };
            var userId = new Data.UserMethods().RegisterUser(user);


            var otherUser = new RegistrationModel()
            {
                Email = "abc1@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };

            var otherUserId = new Data.UserMethods().RegisterUser(otherUser);



            new TimeDifference.Services.UserManager.UserInformation(new UserModel
            {
                Email = user.Email,
                Role = user.RoleId,
                UserId = userId,
                UserName = user.UserName
            });

            // Arrange
            var controller = new TimeZoneEntryManagementController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var testData = new TimeZoneEntryModel
            {
                City = "City2",
                MinuteDifference = 50,
                EntryName = "Entry 2",
                HourDifference = 1,
                UserId = otherUserId,
                TimeZoneEntryId = -1
            };
            var response = controller.SaveTimeZone(testData);

            // Assert
            Assert.Fail("Invalid ID Provided");
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Invalid Hour & Minute provided")]
        public void SaveTimeZone_ShouldGetException_InvalidHourORMinute()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.Admin,
                UserName = "Test User"

            };
            var userId = new Data.UserMethods().RegisterUser(user);

            var otherUser = new RegistrationModel()
            {
                Email = "abc1@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };

            var otherUserId = new Data.UserMethods().RegisterUser(otherUser);


            //Adding TimeZoneInformation
            var timeZoneId1 = new Data.TimeZoneEntryMethods().SaveNewTimeZoneEntry(new TimeZoneEntry
            {

                City = "City1",
                EntryName = "Entry 1",
                UserId = otherUserId,
                Difference = new TimeSpan(5, 30, 0).Ticks,
                IsActive = true
            });


            new TimeDifference.Services.UserManager.UserInformation(new UserModel
            {
                Email = user.Email,
                Role = user.RoleId,
                UserId = userId,
                UserName = user.UserName
            });

            // Arrange
            var controller = new TimeZoneEntryManagementController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.SaveTimeZone(new TimeZoneEntryModel
            {
                City = "City1",
                HourDifference = -19,
                MinuteDifference = -61,
                UserId = userId,
                EntryName = "Entry1",
                TimeZoneEntryId = timeZoneId1

            });

            // Assert
            Assert.Fail("HourDifference or Minute Difference not valid.");
        }

        #endregion

        #region DeleteTimeZoneEntry

        [TestMethod]
        public void DeleteTimeZoneEntry_ShouldDeleteTimeZoneInformation()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.Admin,
                UserName = "Test User"

            };
            var userId = new Data.UserMethods().RegisterUser(user);

            var otherUser = new RegistrationModel()
            {
                Email = "abc1@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };

            var otherUserId = new Data.UserMethods().RegisterUser(otherUser);




            var testTimeZoneId = new Data.TimeZoneEntryMethods().SaveNewTimeZoneEntry(new TimeZoneEntry
            {

                City = "City1",
                EntryName = "Entry 1",
                UserId = otherUserId,
                Difference = new TimeSpan(5, 30, 0).Ticks,
                IsActive = true
            });





            new TimeDifference.Services.UserManager.UserInformation(new UserModel
            {
                Email = user.Email,
                Role = user.RoleId,
                UserId = userId,
                UserName = user.UserName
            });

            // Arrange


            var controller = new TimeZoneEntryManagementController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var savedTimeZoneId = controller.DeleteTimeZoneEntry(testTimeZoneId);

            // Assert
            //Getting Saved Information From DataBase
            var timeZoneInformation = new Data.TimeZoneEntryMethods().GetTimeZoneEntry(testTimeZoneId);

            Assert.AreEqual(timeZoneInformation, null);


        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Invalid ID Provided")]
        public void DeleteTimeZoneEntry_ShouldGetExceptionInvalidID()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.Admin,
                UserName = "Test User"

            };
            var userId = new Data.UserMethods().RegisterUser(user);


            new TimeDifference.Services.UserManager.UserInformation(new UserModel
            {
                Email = user.Email,
                Role = user.RoleId,
                UserId = userId,
                UserName = user.UserName
            });

            // Arrange
            var controller = new TimeZoneEntryManagementController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.DeleteTimeZoneEntry(-1);

            // Assert
            Assert.Fail("Invalid ID Provided");
        }

        #endregion
    }
}

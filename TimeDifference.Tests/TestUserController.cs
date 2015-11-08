using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Contains test cases for User Controller
    /// </summary>
    [TestClass]
    public class TestUserController
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

        #region GetUserInformation
        [TestMethod]
        public void GetUserInformation_ShouldGetUserInformation()
        {

            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
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
            var controller = new UserController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetUserInformation();

            // Assert
            Assert.AreEqual(response.Email, "abc@gmail.com");

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Invalid ID Provided")]
        public void GetUserInformation_ShouldGetException()
        {

            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };


            new TimeDifference.Services.UserManager.UserInformation(new UserModel
            {
                Email = user.Email,
                Role = user.RoleId,
                UserId = -1,
                UserName = user.UserName
            });

            // Arrange
            var controller = new UserController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetUserInformation();

            // Assert
            Assert.Fail("Test Passed");

        }
        #endregion

        #region CheckEmailID
        [TestMethod]
        public void CheckEmailId_ShouldReturnTrueAsEmailAvailable()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
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
            var controller = new UserController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.IsEmailExist(user.Email);

            // Assert
            Assert.AreEqual(true, response);
        }

        [TestMethod]
        public void CheckEmailId_ShouldReturnFalseAsEmailNotAvailable()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
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
            var controller = new UserController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.IsEmailExist("NoEmailAddressExist@gmail.com");

            // Assert
            Assert.AreEqual(false, response);
        }
        #endregion

        #region SaveTimeZone
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Invalid Hour & Minute provided")]
        public void SaveTimeZone_ShouldGetException()
        {

            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
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
            var controller = new UserController()
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

            });

            // Assert
            Assert.Fail("HourDifference or Minute Difference not valid.");
        }

        [TestMethod]
        public void SaveTimeZone_ShouldSaveTheTimeZoneInformationInformation()
        {

            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
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
            var controller = new UserController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act.
            var timeZoneInfo = new TimeZoneEntryModel
            {
                City = "City1",
                HourDifference = 5,
                MinuteDifference = 30,
                UserId = userId,
                EntryName = "Entry1",

            };
            var responseId = controller.SaveTimeZone(timeZoneInfo);

            // Assert
            var timeZoneInformation = new Data.TimeZoneEntryMethods().GetTimeZoneEntry(responseId);
            Assert.AreEqual(timeZoneInformation.City, timeZoneInfo.City);
            Assert.AreEqual(timeZoneInformation.EntryName, timeZoneInfo.EntryName);

            Assert.AreEqual(new TimeSpan(timeZoneInformation.Difference).Minutes, timeZoneInfo.MinuteDifference);
            Assert.AreEqual(new TimeSpan(timeZoneInformation.Difference).Hours, timeZoneInfo.HourDifference);

        }


        [TestMethod]
        public void SaveTimeZone_ShouldModifyExistingTimeZoneInformation()
        {

            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };
            var userId = new Data.UserMethods().RegisterUser(user);

            var timeZoneEntryId = new Data.TimeZoneEntryMethods().SaveNewTimeZoneEntry(new TimeZoneEntry
            {
                City = "City2",
                Difference = new TimeSpan(5, 30, 0).Ticks,
                UserId = userId,
                EntryName = "Entry112",
                IsActive = true,
            });

            new TimeDifference.Services.UserManager.UserInformation(new UserModel
            {
                Email = user.Email,
                Role = user.RoleId,
                UserId = userId,
                UserName = user.UserName
            });






            // Arrange
            var controller = new UserController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var testData = new TimeZoneEntryModel
            {
                City = "City1",
                HourDifference = 9,
                MinuteDifference = 10,
                UserId = userId,
                EntryName = "Entry1",
                TimeZoneEntryId = timeZoneEntryId

            };
            var response = controller.SaveTimeZone(testData);




            // Assert
            //Getting Data from Database to verify
            var timeZoneInformation = new Data.TimeZoneEntryMethods().GetTimeZoneEntry(timeZoneEntryId);
            Assert.AreEqual(timeZoneInformation.City, testData.City);
            Assert.AreEqual(timeZoneInformation.EntryName, testData.EntryName);

            Assert.AreEqual(new TimeSpan(timeZoneInformation.Difference).Minutes, testData.MinuteDifference);
            Assert.AreEqual(new TimeSpan(timeZoneInformation.Difference).Hours, testData.HourDifference);

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Invalid TimeZoneId")]
        public void SaveTimeZone_ShouldReturnExceptionInvalidTimeZoneId()
        {

            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
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
            var controller = new UserController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var testData = new TimeZoneEntryModel
            {
                City = "City1",
                HourDifference = 5,
                MinuteDifference = 30,
                UserId = userId,
                EntryName = "Entry1",
                TimeZoneEntryId = -12

            };
            var response = controller.SaveTimeZone(testData);

            //Assert
            Assert.Fail("Invalid TimeZone ID");



        }



        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "UnAuthorize")]
        public void SaveTimeZone_ShouldReturnExceptionUnAuthorize()
        {

            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };


            var otherUser = new RegistrationModel()
            {
                Email = "abc1@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };
            var userId = new Data.UserMethods().RegisterUser(user);
            var otherUserId = new Data.UserMethods().RegisterUser(otherUser);

            var timeZoneEntryId = new Data.TimeZoneEntryMethods().SaveNewTimeZoneEntry(new TimeZoneEntry
            {
                City = "City2",
                Difference = 108000000000,
                UserId = userId,
                EntryName = "Entry112",
                IsActive = true,
            });

            new TimeDifference.Services.UserManager.UserInformation(new UserModel
            {
                Email = otherUser.Email,
                Role = otherUser.RoleId,
                UserId = otherUserId,
                UserName = otherUser.UserName
            });






            // Arrange
            var controller = new UserController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var testData = new TimeZoneEntryModel
            {
                City = "City1",
                HourDifference = 5,
                MinuteDifference = 30,
                UserId = otherUserId,
                EntryName = "Entry1",
                TimeZoneEntryId = timeZoneEntryId

            };
            var response = controller.SaveTimeZone(testData);
            // Assert
            Assert.Fail("UnAuthorize");



        }

        #endregion

        #region DeleteTimeZone
        [TestMethod]
        public void DeleteTimeZoneEntry_ShouldDeleteTimZoneInformation()
        {

            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };
            var userId = new Data.UserMethods().RegisterUser(user);

            var timeZoneEntryId = new Data.TimeZoneEntryMethods().SaveNewTimeZoneEntry(new TimeZoneEntry
            {
                City = "City2",
                Difference = 108000000000,
                UserId = userId,
                EntryName = "Entry112",
                IsActive = true,
            });

            new TimeDifference.Services.UserManager.UserInformation(new UserModel
            {
                Email = user.Email,
                Role = user.RoleId,
                UserId = userId,
                UserName = user.UserName
            });

            // Arrange
            var controller = new UserController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act

            var response = controller.DeleteTimeZoneEntry(timeZoneEntryId);
            // Assert
            //Getting Data from Database to verify
            var deleteInfoFromDataBase = new Data.TimeZoneEntryMethods().GetTimeZoneEntry(timeZoneEntryId);
            Assert.AreEqual(deleteInfoFromDataBase,null);


        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Invalid TimeZoneId")]
        public void DeleteTimeZoneEntry_ShouldReturnExceptionInvalidTimeZoneID()
        {

            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
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
            var controller = new UserController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var testData = new TimeZoneEntryModel
            {
                City = "City1",
                HourDifference = 5,
                MinuteDifference = 30,
                UserId = userId,
                EntryName = "Entry1",
                TimeZoneEntryId = -12

            };
            var response = controller.SaveTimeZone(testData);

            //Assert
            Assert.Fail("Invalid TimeZone ID");


        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "UnAuthorize")]
        public void DeleteTimeZoneEntry_ShouldReturnExceptionUnAuthorize()
        {

            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };


            var otherUser = new RegistrationModel()
            {
                Email = "abc1@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };
            var userId = new Data.UserMethods().RegisterUser(user);
            var otherUserId = new Data.UserMethods().RegisterUser(otherUser);

            var timeZoneEntryId = new Data.TimeZoneEntryMethods().SaveNewTimeZoneEntry(new TimeZoneEntry
            {
                City = "City2",
                Difference = 108000000000,
                UserId = userId,
                EntryName = "Entry112",
                IsActive = true,
            });

            new TimeDifference.Services.UserManager.UserInformation(new UserModel
            {
                Email = otherUser.Email,
                Role = otherUser.RoleId,
                UserId = otherUserId,
                UserName = otherUser.UserName
            });






            // Arrange
            var controller = new UserController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.DeleteTimeZoneEntry(timeZoneEntryId);
            // Assert
            Assert.Fail("UnAuthorize");



        }

        #endregion

        #region GetTimeZoneInformation

        [TestMethod]
        public void GetTimeZoneInformation_ShouldReturnTimeZoneInformation()
        {

            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };
            var userId = new Data.UserMethods().RegisterUser(user);

            var timeZoneEntryId = new Data.TimeZoneEntryMethods().SaveNewTimeZoneEntry(new TimeZoneEntry
            {
                City = "City2",
                Difference = 108000000000,
                UserId = userId,
                EntryName = "Entry112",
                IsActive = true,
            });

            new TimeDifference.Services.UserManager.UserInformation(new UserModel
            {
                Email = user.Email,
                Role = user.RoleId,
                UserId = userId,
                UserName = user.UserName
            });






            // Arrange
            var controller = new UserController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act

            var response = controller.GetTimeZoneInformation(timeZoneEntryId);




            // Assert
            //Getting Data from Database to verify
            var timeZoneInformation = controller.GetTimeZoneInformation(timeZoneEntryId);
            Assert.AreEqual(timeZoneInformation.HourDifference, response.HourDifference);
            Assert.AreEqual(timeZoneInformation.MinuteDifference, response.MinuteDifference);
            Assert.AreEqual(timeZoneInformation.City, response.City);
            Assert.AreEqual(timeZoneInformation.EntryName, response.EntryName);

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "UnAuthorize")]
        public void GetTimeZoneInformation_ShouldReturnExceptionUnAuthorize()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };


            var otherUser = new RegistrationModel()
            {
                Email = "abc1@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };
            var userId = new Data.UserMethods().RegisterUser(user);
            var otherUserId = new Data.UserMethods().RegisterUser(otherUser);

            var timeZoneEntryId = new Data.TimeZoneEntryMethods().SaveNewTimeZoneEntry(new TimeZoneEntry
            {
                City = "City2",
                Difference = 108000000000,
                UserId = userId,
                EntryName = "Entry112",
                IsActive = true,
            });

            new TimeDifference.Services.UserManager.UserInformation(new UserModel
            {
                Email = otherUser.Email,
                Role = otherUser.RoleId,
                UserId = otherUserId,
                UserName = otherUser.UserName
            });






            // Arrange
            var controller = new UserController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetTimeZoneInformation(otherUserId);
            // Assert
            Assert.Fail("UnAuthorize");


        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "TimeZoneIdNotFound")]
        public void GetTimeZoneInformation_ShouldReturnExceptionTimeZoneIdNotFound()
        {

            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
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
            var controller = new UserController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetTimeZoneInformation(-12);

            //Assert
            Assert.Fail("Invalid TimeZone ID");


        }

        #endregion

        #region GetAllTimeZone
        [TestMethod]
        public void GetAllTimeZone_ShouldReturnAllTimeZoneForUser()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
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
            var controller = new UserController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetAllTimeZone(1, 1000);

            // Assert
            List<TimeZoneEntryModel> products = response;
            Assert.AreEqual(0, products.Count);
        }
        #endregion

        #region ModifyUserInformation

        [TestMethod]
        public void ModifyUserInformation_ShouldModifyUserInformation()
        {

            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
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
            var controller = new UserController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.ModifyUserInformation(new UserModel
            {
                Email = "abc12@gmail.com",
                Role = UserRole.User,
                UserName = "Test User1"

            });

            // Assert
            //getting UserInfo from DataBase
            var userInfo = new Data.UserMethods().GetUserInformationBasedOnId(userId);

            Assert.AreEqual(userInfo.UserName, "Test User1");
            Assert.AreEqual(userInfo.Email, "abc12@gmail.com");

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Bad Request")]
        public void ModifyUserInformation_ShouldReturnExceptionAlreadyUsedEmailId()
        {

            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
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
            var controller = new UserController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.ModifyUserInformation(new UserModel
            {
                Email = "abc1@gmail.com",
                Role = UserRole.User,
                UserName = "Test User"

            });

            // Assert
            Assert.Fail("Bad Request Already Used Email");

        }
        #endregion

        #region RegisterUser

        [TestMethod]
        public void RegisterUser_ShouldRegisterUser()
        {

            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };
            var userId = new Data.UserMethods().RegisterUser(user);




            // Arrange
            var controller = new UserController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var otherUser = new RegistrationModel()
            {
                Email = "abc1@gmail.com",
                Password = "123456789",
                RoleId = UserRole.User,
                UserName = "Test User"

            };
            var response = controller.RegisterUser(otherUser);

            // Assert
            var userInfoFromDatabase = new Data.UserMethods().GetUserInformationBasedOnId(response);

            Assert.AreEqual(userInfoFromDatabase.Email, otherUser.Email);
            Assert.AreEqual(userInfoFromDatabase.UserName, otherUser.UserName);
            
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Bad Request")]
        public void RegisterUser_ShouldReturnExceptionAlreadyUsedEmailId()
        {

            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };
            var userId = new Data.UserMethods().RegisterUser(user);




            // Arrange
            var controller = new UserController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var otherUser = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "123456789",
                RoleId = UserRole.User,
                UserName = "Test User"

            };
            var response = controller.RegisterUser(otherUser);

            // Assert
            Assert.Fail("Bad Request Already Used Email");

        }
        #endregion
    }
}


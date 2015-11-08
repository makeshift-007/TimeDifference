using System;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeDifference.BusinessClasses;
using TimeDifference.Services.Controllers;

namespace TimeDifference.Tests
{
    [TestClass]
    public class TestUserManagementController
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


        #region GetUserInformationByID
        [TestMethod]
        public void GetUserInformationById_ShouldGetUserInformation()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.Manager,
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
            var controller = new UserManagementController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetUserInformationById(otherUserId);

            // Assert
            Assert.AreEqual(response.Email, "abc1@gmail.com");
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Invalid ID Provided")]
        public void GetUserInformationById_ShouldGetExceptionInvalidID()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.Manager,
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
            var controller = new UserManagementController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetUserInformationById(-1);

            // Assert
            Assert.Fail("Invalid User ID");
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
                RoleId = UserRole.Manager,
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
            var controller = new UserManagementController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var newUserInfo = new UserModel
            {
                Email = "abc2@gmail.com",
                Role = UserRole.User,
                UserName = "Test User 11",
                UserId = otherUserId
            };
            var response = controller.ModifyUserInformation(newUserInfo);

            // Assert
            //getting Information from database
            var userInfo = new Data.UserMethods().GetUserInformationBasedOnId(otherUserId);
            Assert.AreEqual(userInfo.Email, newUserInfo.Email);
            Assert.AreEqual(userInfo.Role, newUserInfo.Role);
            Assert.AreEqual(userInfo.UserName, newUserInfo.UserName);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "UnAuthorize")]
        public void ModifyUserInformation_ShouldGetExceptionUnAuthorize()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.Manager,
                UserName = "Test User"

            };
            var userId = new Data.UserMethods().RegisterUser(user);

            var otherUser = new RegistrationModel()
            {
                Email = "abc1@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.Admin,
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
            var controller = new UserManagementController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var newUserInfo = new UserModel
            {
                Email = "abc2@gmail.com",
                Role = UserRole.User,
                UserName = "Test User 11",
                UserId = otherUserId
            };
            var response = controller.ModifyUserInformation(newUserInfo);

            // Assert
            Assert.Fail("UnAuthorize");
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Bad Request")]
        public void ModifyUserInformation_ShouldGetExceptionEmailAlreadyExist()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.Manager,
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


            var anotherUser = new RegistrationModel()
            {
                Email = "abc2@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };

            var anotherUserId = new Data.UserMethods().RegisterUser(anotherUser);


            new TimeDifference.Services.UserManager.UserInformation(new UserModel
            {
                Email = user.Email,
                Role = user.RoleId,
                UserId = userId,
                UserName = user.UserName
            });

            // Arrange
            var controller = new UserManagementController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var newUserInfo = new UserModel
            {
                Email = "abc2@gmail.com",
                Role = UserRole.User,
                UserName = "Test User 11",
                UserId = otherUserId
            };
            var response = controller.ModifyUserInformation(newUserInfo);

            // Assert
            Assert.Fail("Bad Request");
        }

        #endregion

        #region GetAllUsers
        [TestMethod]
        public void GetAllUsers_ShouldGetRecordsForAllUser()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.Manager,
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
            var controller = new UserManagementController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.GetAllUsers(1, 10);

            // Assert
            Assert.IsTrue(response.Count >= 1);

        }

        #endregion

        #region RemoveUser
        [TestMethod]
        public void RemoveUser_ShouldRemoveUserInformation()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.Manager,
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
            var controller = new UserManagementController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.RemoveUser(otherUserId);

            // Assert
            //getting Information from database
            var userInfo = new Data.UserMethods().GetUserInformationBasedOnId(otherUserId);
            Assert.IsTrue(userInfo==null);
        } 

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "UnAuthorize")]
        public void RemoveUser_ShouldGetExceptionUnAuthorize()
        {
            //Setting Up Resources
            var user = new RegistrationModel()
            {
                Email = "abc@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.Manager,
                UserName = "Test User"

            };
            var userId = new Data.UserMethods().RegisterUser(user);

            var otherUser = new RegistrationModel()
            {
                Email = "abc1@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.Admin,
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
            var controller = new UserManagementController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.RemoveUser(otherUserId);

            // Assert
            Assert.Fail("UnAuthorize");
        }

        #endregion

        #region ModifyUserInformationAdmin


        [TestMethod]
        public void ModifyUserInformationAdmin_ShouldModifyUserInformation()
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
            var controller = new UserManagementController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var newUserInfo = new UserModel
            {
                Email = "abc2@gmail.com",
                Role = UserRole.Manager,
                UserName = "Test User 11",
                UserId = otherUserId
            };
            var response = controller.ModifyUserInformationAdmin(newUserInfo);

            // Assert
            //getting Information from database
            var userInfo = new Data.UserMethods().GetUserInformationBasedOnId(otherUserId);
            Assert.AreEqual(userInfo.Email, newUserInfo.Email);
            Assert.AreEqual(userInfo.Role, newUserInfo.Role);
            Assert.AreEqual(userInfo.UserName, newUserInfo.UserName);
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "Bad Request")]
        public void ModifyUserInformationAdmin_ShouldGetExceptionEmailAlreadyExist()
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


            var anotherUser = new RegistrationModel()
            {
                Email = "abc2@gmail.com",
                Password = "049105218045123089094000010226080017120086080007",
                RoleId = UserRole.User,
                UserName = "Test User"

            };

            var anotherUserId = new Data.UserMethods().RegisterUser(anotherUser);


            new TimeDifference.Services.UserManager.UserInformation(new UserModel
            {
                Email = user.Email,
                Role = user.RoleId,
                UserId = userId,
                UserName = user.UserName
            });

            // Arrange
            var controller = new UserManagementController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var newUserInfo = new UserModel
            {
                Email = "abc2@gmail.com",
                Role = UserRole.User,
                UserName = "Test User 11",
                UserId = otherUserId
            };
            var response = controller.ModifyUserInformationAdmin(newUserInfo);

            // Assert
            Assert.Fail("Bad Request");
        }


        #endregion 

    }
}

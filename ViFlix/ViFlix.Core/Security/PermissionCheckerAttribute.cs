//using FirstShop.Core.Services.User.PermissionServices;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using System;
//using System.Collections.Generic;
//using System.Security.Cryptography;
//using System.Text;

//namespace shop.Core.Security
//{

//    public class PermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
//    {
//        private IPermissionServices _permissionService;
//        private int _permissionId = 0;
//        public PermissionCheckerAttribute(int permissionId)
//        {
//            _permissionId = permissionId;
//        }

//        public void OnAuthorization(AuthorizationFilterContext context)
//        {
//            _permissionService =
//                (IPermissionServices)context.HttpContext.RequestServices.GetService(typeof(IPermissionServices));
//            if (context.HttpContext.User.Identity.IsAuthenticated)
//            {
//                string userName = context.HttpContext.User.Identity.Name;

//                if (!_permissionService.CheckPermission(_permissionId, userName))
//                {
//                    context.Result = new RedirectResult("/Account/Login");
//                }
//            }
//            else
//            {
//                //context.Result = new RedirectResult("/Account/Login");
//            }
//        }
//    }
//}
// Copyright 2012 Max Toro Q.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;

namespace MvcContact {
   
   [OutputCache(Location = OutputCacheLocation.None)]
   public class ContactController : Controller {

      ContactSender service;

      public ContactController() { }

      public ContactController(ContactSender service) {
         this.service = service;
      }

      protected override void Initialize(RequestContext requestContext) {
         
         base.Initialize(requestContext);

         ContactConfiguration config = ContactConfiguration.Current(requestContext);

         this.service = config.RequireDependency(this.service);
      }

      [HttpGet]
      public ActionResult Index() {

         this.ViewData.Model = new IndexViewModel(this.service.Send());
         
         return View();
      }

      [HttpPost]
      public ActionResult Index(string foo) {

         ContactInput input = this.service.CreateContactInput();

         this.ViewData.Model = new IndexViewModel(input);
         
         if (!ModelBinderUtil.TryUpdateModel(input, this)) {
            
            this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return View();
         }

         if (!this.service.Send(input, this.ControllerContext)) {
            
            this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View();
         }

         return RedirectToAction("Success");
      }

      [HttpGet]
      public ActionResult Success() {
         return View();
      }
   }
}

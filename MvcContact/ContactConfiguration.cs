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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace MvcContact {
   
   public class ContactConfiguration {

      // IMPORTANT: When new instance properties are added Import and Reset must be updated.

      static readonly ContactConfiguration Default = new ContactConfiguration(defaultsConstructor: true);

      public string From { get; set; }
      public string To { get; set; }
      public string CC { get; set; }
      public string Bcc { get; set; }
      public Func<ContactSender> ContactSenderResolver { get; set; }
      public string ResourceClassKey { get; set; }

      internal static ContactConfiguration Current(RequestContext requestContext = null) {

         if (requestContext == null) {

            HttpContext httpContext = HttpContext.Current;

            if (httpContext != null) {
               requestContext = httpContext.Request.RequestContext;
            }
         }

         if (requestContext == null) {
            throw new InvalidOperationException();
         }

         return requestContext.RouteData.DataTokens["Configuration"] as ContactConfiguration
            ?? Default;
      }

      public ContactConfiguration()
         : this(Default) { }

      public ContactConfiguration(ContactConfiguration config) {

         if (config == null) throw new ArgumentNullException("config");

         Import(config);
      }

      private ContactConfiguration(bool defaultsConstructor) {
         Reset();
      }

      public void Reset() {

         this.From = null;
         this.To = null;
         this.CC = null;
         this.Bcc = null;
         this.ContactSenderResolver = null;
         this.ResourceClassKey = null;
      }

      void Import(ContactConfiguration config) {

         this.From = config.From;
         this.To = config.To;
         this.CC = config.CC;
         this.Bcc = config.Bcc;
         this.ContactSenderResolver = config.ContactSenderResolver;
         this.ResourceClassKey = config.ResourceClassKey;
      }

      internal ContactSender RequireDependency(ContactSender injectedDependency) {

         Func<ContactSender> resolver = this.ContactSenderResolver;

         ContactSender instance = (resolver != null) ?
            resolver()
            : injectedDependency;

         if (instance != null) {
            instance.Configuration = this;
         } else {
            instance = new ContactSender(this);
         }

         return instance;
      }
   }
}

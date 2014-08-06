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
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;

namespace MvcContact {
   
   public class ContactSender {

      readonly SmtpClient smtpClient;
      ContactConfiguration config;
      
      protected internal ContactConfiguration Configuration {
         get { return config; }
         internal set { config = value; }
      }

      public ContactSender() 
         : this(ContactConfiguration.Current()) { }

      public ContactSender(ContactConfiguration config) 
         : this(config, new SmtpClient()) { }

      public ContactSender(ContactConfiguration config, SmtpClient smtpClient) {

         if (config == null) throw new ArgumentNullException("config");
         if (smtpClient == null) throw new ArgumentNullException("smtpClient");

         this.config = config;
         this.smtpClient = smtpClient;
      }

      public virtual ContactInput CreateContactInput() {
         return new ContactInput();
      }

      protected virtual void InitializeContactInput(ContactInput input) { }

      public ContactInput Send() {

         ContactInput input = CreateContactInput();
         InitializeContactInput(input);

         return input;
      }

      public virtual bool Send(ContactInput input, ControllerContext context) {

         var message = new MailMessage {
            To = { this.config.To },
            ReplyToList = { new MailAddress(input.Email, input.Name) },
            Subject = input.Subject,
            Body = RenderViewAsString(context, "_Mail", input)
         };

         if (this.config.From != null) {
            message.From = new MailAddress(this.config.From);
         }

         if (this.config.CC != null) {
            message.CC.Add(this.config.CC);
         }

         if (this.config.Bcc != null) {
            message.Bcc.Add(this.config.Bcc);
         }

         try {
            this.smtpClient.Send(message);

         } catch (SmtpException ex) {
            
            LogException(ex);

            return false;
         }

         return true;
      }

      string RenderViewAsString(ControllerContext context, string viewName, object model) {

         ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(context, viewName);

         if (viewResult.View == null) {
            throw new InvalidOperationException();
         }

         using (var output = new StringWriter()) {

            var viewContext = new ViewContext(
               context,
               viewResult.View,
               new ViewDataDictionary(model),
               new TempDataDictionary(),
               output
            );

            viewResult.View.Render(viewContext, output);

            return output.ToString();
         }
      }

      protected void LogException(Exception exception) { }
   }
}

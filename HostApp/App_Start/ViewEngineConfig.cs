﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcCodeRouting;

namespace HostApp {
   
   public class ViewEngineConfig {

      public static void RegisterViewEngines(ViewEngineCollection viewEngines) {
         viewEngines.EnableCodeRouting();
      }
   }
}
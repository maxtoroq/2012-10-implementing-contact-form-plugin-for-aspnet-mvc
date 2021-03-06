﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HostApp.Models {

   public class CustomContactInput : MvcContact.ContactInput {

      [Required]
      [Display(Name = "How did you hear about us?", Order = 3)]
      [UIHint("Source")]
      public virtual string Source { get; set; }
   }
}
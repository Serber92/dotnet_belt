using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Belt.Models
{
    public class Wrapper
    {
        public User User {get; set; }
        public List<User> Users {get; set; }
        public _Activity Activity {get; set; }
        public List<_Activity> Activities {get; set; }
        public Participant Participant {get; set; }
        public List<Participant> Participants {get; set; }
        public Login Login {get; set; }
    }
}
﻿using PhlegmaticOne.InnoGotchi.Data.Models.Base;

namespace PhlegmaticOne.InnoGotchi.Data.Models;

public class UserProfile : ModelBase
{
    public DateTime JoinDate { get; set; }
    public string FirstName { get; set; } = null!;
    public string SecondName { get; set; } = null!;
    public byte[] AvatarData { get; set; } = Array.Empty<byte>();
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Farm Farm { get; set; }
    public IEnumerable<Collaboration> Collaborations { get; set; }
    public IEnumerable<Invitation> SentInvitations { get; set; }
    public IEnumerable<Invitation> ReceivedInvitations { get; set; }
}
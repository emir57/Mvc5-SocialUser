# Mvc5-SocialUser
(Data Layer)⚙️<br>
(Business Layer)⚙️<br>
(Presentation Layer)⚙️<br>
Async Programming ⚙️<br>
Dependency Injection ⚙️<br>
using AJAX ⚙️<br>
using SignalR ⚙️<br>
using Microsoft Identity ⚙️<br>
real time chat and group chat ⚙️<br>
<br>
Tables📋<br><br>
User
<table>
  <tr>
  <td>Id</td>
  <td>firstName</td>
  <td>lastName</td>
  <td>DateOfBirth</td>
  <td>profilePhoto</td>
  <td>Email</td>
  <td>EmailConfirmed</td>
  <td>PasswordHash</td>
  <td>SecurityStamp</td>
  <td>PhoneNumber</td>
  <td>PhoneNumberConfirmed</td>
  <td>TwoFactorEnabled</td>
  <td>LockoutEndDateUtc</td>
  <td>LockoutEnabled</td>
  <td>AccessFailedCount</td>
  <td>UserName</td>
  <td>profileDescription</td>
  </tr>
</table>
Post
<table>
  <tr>
  <td>PostId</td>
  <td>UserId</td>
  <td>Username</td>
  <td>UserProfilePhoto</td>
  <td>PostPicture</td>
  <td>Description</td>
  <td>LikeCount</td>
  <td>PostDateTime</td>
  </tr>
</table>
Role
<table>
  <tr>
  <td>Id</td>
  <td>Name</td>
  </tr>
</table>
UserRoles
<table>
  <tr>
  <td>UserId</td>
  <td>RoleId</td>
  </tr>
</table>
PostLike
<table>
  <tr>
  <td>Id</td>
  <td>PostId</td>
  <td>UserId</td>
  </tr>
</table>
UserFriend
<table>
  <tr>
  <td>Id</td>
  <td>UserId1</td>
  <td>UserId2</td>
  <td>CheckFriend</td>
  </tr>
</table>
ChatMessage
<table>
  <tr>
  <td>Id</td>
  <td>SenderMessageUser</td>
  <td>RecipientMessageUser</td>
  <td>MessageText</td>
  <td>MessageDateTime</td>
  </tr>
</table>
Group
<table>
  <tr>
  <td>GroupId</td>
  <td>GroupName</td>
  <td>GroupDateTime</td>
  <td>CreateGroupUserId</td>
  </tr>
</table>
GroupMessage
<table>
  <tr>
  <td>MessageId</td>
  <td>GroupId</td>
  <td>SenderUserId</td>
  <td>Message</td>
  <td>MessageDateTime</td>
  </tr>
</table>
GroupMember
<table>
  <tr>
  <td>MemberId</td>
  <td>GroupId</td>
  <td>UserId</td>
  <td>Role</td>
  </tr>
</table>
GroupMessage
<table>
  <tr>
  <td>MessageId</td>
  <td>GroupId</td>
  <td>SenderUserId</td>
  <td>Message</td>
  <td>MessageDateTime</td>
  </tr>
</table>
Comment
<table>
  <tr>
  <td>Id</td>
  <td>PostId</td>
  <td>UserName</td>
  <td>UserId</td>
  <td>CommentDescription</td>
  <td>CommentDateTime</td>
  </tr>
</table>
CommentAnswer
<table>
  <tr>
  <td>Id</td>
  <td>CommentId</td>
  <td>UserName</td>
  <td>UserId</td>
  <td>AnswerDescription</td>
  <td>AnswerDateTime</td>
  </tr>
</table>
⭐Photos⭐
<br>
<img width="500" src="https://i.ibb.co/xsJRKPK/register.png">
<img width="500" src="https://i.ibb.co/cQVpR09/login.png">
<img width="500" src="https://i.ibb.co/0tdRdhj/postpaylas.png">
<img width="500" src="https://i.ibb.co/2vgW2cD/1.png">
<img width="500" src="https://i.ibb.co/Sw7RP2c/detay.png">
<img width="500" src="https://i.ibb.co/jGCqjKP/detay2.png">
<img width="500" src="https://i.ibb.co/w0FWPg8/profile-git.png">
<img width="500" src="https://i.ibb.co/GxJn6PC/profil.png">
<img width="500" src="https://i.ibb.co/bX9QxQQ/sayfam.png">
<img width="500" src="https://i.ibb.co/PN2W4Qs/sayfam2.png">
<img width="500" src="https://i.ibb.co/DGXrDbP/grupolu-tur.png">
<img width="500" src="https://i.ibb.co/rFMsbTC/chat1.png">
<img width="500" src="https://i.ibb.co/VNjvfWZ/chat2.png">


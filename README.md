# Mvc5-SocialUser
Mvc 5 ile yapmış olduğum sosyal medya sitesi<br>
Async Programming ⚙️<br>
using AJAX ⚙️<br>
using SignalR ⚙️<br>
using Microsoft Identity ⚙️<br>
real time chat ⚙️<br>
<br>
Tables📋
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
Post
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
  <td>[Check]</td>
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

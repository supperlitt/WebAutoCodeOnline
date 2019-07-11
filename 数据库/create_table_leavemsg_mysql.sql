create table LeaveMsg
(
	Id int primary key auto_increment,
	NickName varchar(50) not null,
	Email varchar(200) not null,
	Msg varchar(500) not null,
	IP varchar(15) not null,
	LeaveTime datetime not null,
	IsShow tinyint not null
);

create table OpLog
(
	Id int primary key auto_increment,
	OpType tinyint not null,
	OpContent varchar(500) not null,
	OpIP varchar(15) not null,
	OpTime datetime not null
);

create table UserInfo
(
	Id int primary key auto_increment,
	UserName varchar(50) not null,
	UserPwd varchar(50) not null,
	LastLoginTime datetime not null
);

create table Article
(
	Id int primary key auto_increment,
	Title varchar(100) not null,
	Content text not null,
	Tags varchar(100) not null,
	GroupIds int not null, -- 分组，使用的二进制交集
	PublishTime datetime not null,
	LastChangeTime datetime not null,
	IsShow tinyint not null, -- 是否显示，
	IsTop tinyint not null, -- 是否置顶
);
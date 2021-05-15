create database mydemo

use mydemo

create table product(
	Id int primary key,
	Name nvarchar(50),
	Price money,
	Quantity int,
	CreationDate date
)

INSERT INTO Product (Id , Name , Price , Quantity , CreationDate) VALUES (1 , 'Name1' , 10000 , 50 , '')
INSERT INTO Product (Id , Name , Price , Quantity , CreationDate) VALUES (2 , 'Name2' , 15000 , 30 , '')

DELETE from Product WHERE Id = 1

select * from Product
drop table Product

CREATE TABLE tblLogin (
    id int IDENTITY(1,1) primary key,
    username varchar(255),
    password varchar(255),
    role int,
);

insert into tblLogin (username , password) values ('musang', 1234)
insert into tblLogin (username , password, role) values ('king', 1234 ,'Rumah Sakit')
insert into tblLogin (username , password, role) values ('produsen', 2345 ,'Produsen')

select * from tblLogin

ALTER TABLE tblLogin
    ALTER COLUMN role varchar(15);
Create table Login 
(
username varchar(50) Not Null Primary Key,
userpassword varchar(50) Not Null
)


Create table Stock
(
 ProductCode int Not Null Primary Key,
 ProductName varchar(50) Not Null,
 TransDate datetime Not Null,
 Quantity float Not Null
)



--For creating DB diagrams
--Alter AUTHORIZATION ON DATABASE::Stock TO [sa];
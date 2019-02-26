# TomasosPizzeria

School project for the course ASP .NET MVC 1.

A webshop for a Pizzeria using .NET Core 2.2 with Core Identity and MSSQL.

The shop has 3 roles (Admin / PremiumUser / RegularUser) that has different benefits and permissions.

- Index will show the menu for the pizzeria, a user can log in or use registration to add a new account.

- As logged in a user can make a new order of dishes.

- The cart is saved as a session until clicking "Beställ"

- Using Core Identity to store users and rolemanager to store roles.


#Admin

- Can see all made orders, mark them as delivered and delete them.

- Can view all customers and change their roles between Premium and Regular.

- Can create and edit dishes and ingredients (change ingredients used, name, price, details)

( To test the Admin account you can login as username: AdminUser password: admin )


#PremiumUser

- Gets 20% discount when ordering more than 3 dishes.

- Gets 10 bonus points per dish ordered, when user has 100 points they will recieve a free pizza in their next order.

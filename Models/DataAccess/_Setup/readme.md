## Setting up your own Database and creating an Admin user for yourself

1. First, make sure you have `docker-compose`, `gcc`, and `cargo` installed
```bash
which docker-compose
which gcc
which cargo
```
If missing, install them via your package-manager.

2. Create a copy of the EXAMPLE.env file, named .env, and edit it to your needs
```bash
cp EXAMPLE.env .env && vim .env
```
Choose a secure password, then delete the .env. Don't worry about accidentally pushing, it is in the .gitignore.

3. Create a new python venv and activate it; install requirements
```bash
python -m venv venv
source venv/bin/activate
pip install -r requirements.txt
```

4. Verify and Generate the user SQL
```bash
python GenerateUser.py
python GenerateUser.py > user.sql
```
Ensure the file is encoded in UTF-8. Running the above command in Powershell, will result in UTF-16, which PostgreSQL is unable to parse.
If you have created the database already, ensure that the user exists by selecting all from the USERS table. If not, copy the user.sql contents into your query editor and run them.

5. Let docker-compose do its thing:
```bash
docker-compose up -d
```

6. Connect to the Database and verify the data user has been created successfully.
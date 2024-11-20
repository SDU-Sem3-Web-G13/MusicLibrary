## Setting up the Database and webapp for deployment.

The following instructions are given for Linux-based systems only. Support on any other platform, even other UNIX-like platforms, is not guaranteed.

1. First, make sure you have `docker `, and `python (version 3.x)` 
```bash
which docker 
which python
python --version
```
If missing, install them via your package-manager.

2. Create a copy of the EXAMPLE.env file contained in the root folder, named .env, and edit it to your needs. Make sure you leave the empty lines at the bottom of the file.
```bash
cp ../EXAMPLE.env ../.env && vim ../.env
```

3. Create a new python venv and activate it; install requirements
```bash
python -m venv venv
source venv/bin/activate
pip install -r requirements.txt
```

4. Generate BCrypt Salt and verify it has been added to your .env
```bash
python GenerateBcryptSalt.py
cat ../.env
```

5. Verify and Generate the user SQL
```bash
python GenerateUser.py
python GenerateUser.py > user.sql
```
Ensure the file is encoded in UTF-8. Running the above command in Powershell will result in UTF-16, which PostgreSQL is unable to parse.
If you have created the database already, ensure that the user exists by selecting all from the USERS table. If not, copy the user.sql contents into your query editor and run them.
Furthermore, if you have started the container without a user.sql to mount, docker will create an empty directory in it's stead, which is write-protected. Make sure you delete this directory with elevated privileges before attempting to generate your user.sql.

6. Let docker-compose do its thing:
```bash
cd ..
docker-compose up -d
```

7. Visit http://127.0.0.1/ in your browser and verify that you can log in with the credentials you specified in the .env. After your user.sql file has been generated, your email and password are securely hashed, and you can delete those entries from your .env.



Notes: The postgres port must be set to 5432 in order for the containerised aspnetapp to connect. the service name needs to be equal to that of the postgres service in docker-compose.yml.
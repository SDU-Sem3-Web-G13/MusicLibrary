import bcrypt
import base64
import os

salt = base64.b64encode(bcrypt.gensalt()).decode('utf-8')
print("Salt: " + salt)
env_path = "../.env"

if(os.path.exists(env_path)):
    with(open(env_path, "a") as f):
        f.write(f"BCRYPT_SALT=\"{salt}\"")
else:
    print(".env file not found. Please copy EXAMPLE.env to .env and try again.")
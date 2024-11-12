from dotenv import load_dotenv
import bcrypt
import os
import base64

def main():
    load_dotenv()
    UMAIL = os.getenv("PGADMIN_DEFAULT_EMAIL")
    UPASS = os.getenv("PGADMIN_DEFAULT_PASSWORD")
    
    # Define a fixed salt. (random salt for now)
    base64salt = os.getenv("BCRYPT_SALT")
    fixed_salt = base64.b64decode(base64salt)
    
    # Hash the email and password using the fixed salt
    UMAIL_HASHED = bcrypt.hashpw(UMAIL.encode('utf-8'), fixed_salt)
    UPASS_HASHED = bcrypt.hashpw(UPASS.encode('utf-8'), fixed_salt)
    
    QUERY1 = str("INSERT INTO USER_CREDENTIALS (UMAIL_HASH, UPASS_HASH) VALUES (decode('" +  str(UMAIL_HASHED.hex()) + "', 'hex'), decode('" +  str(UPASS_HASHED.hex()) + "', 'hex'));")
    QUERY2 = str("INSERT INTO USERS (U_NAME, U_MAIL) VALUES ('Admin', '" + UMAIL + "');")
    
    QUERY = str("BEGIN;\n\n" + QUERY1 + "\n" + QUERY2 + "\n\nCOMMIT;")
    
    print(QUERY)
    return

if __name__ == "__main__":
    main()
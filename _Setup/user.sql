BEGIN;

INSERT INTO USER_CREDENTIALS (UMAIL_HASH, UPASS_HASH) VALUES (decode('2432622431322454785561667a45312e74565661376653422e6576346554314f706471493445504556706c5345537478667a794877756d6379344c4b', 'hex'), decode('2432622431322454785561667a45312e74565661376653422e65763465514e75526c62763261705a4866567667652e6772312e6b594c70447441732e', 'hex'));
INSERT INTO USERS (U_NAME, U_MAIL, U_ISADMIN) VALUES ('Admin', 'admin@admin.com', TRUE);

COMMIT;

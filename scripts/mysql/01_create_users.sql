CREATE USER IF NOT EXISTS 'backend'@'localhost' IDENTIFIED BY '$BACKEND_PWD';

GRANT ALL PRIVILEGES ON development.* TO 'backend'@'localhost';

var crypto = require('crypto');

function getSalt(callback) {
    var result = crypto.randomBytes(16).toString('hex').slice(0, 32)
    callback(result);
};

function hashAndSaltPassword(password) {
    return new Promise(function (resolve, reject) {
        getSalt(function (salt) {
            var hash = crypto.createHmac('sha512', salt);
            hash.update(password);
            var value = hash.digest('hex');
            resolve({
                salt: salt,
                passwordHash: value
            });
        });
    });
};

function verifyPassword(hash, salt, enteredPassword) {
    return new Promise(function (resolve, reject) {
        var hashedSalt = crypto.createHmac('sha512', salt);
        hashedSalt.update(enteredPassword);
        var value = hashedSalt.digest('hex');

        if (value == hash)
            resolve(1);
        else
            reject(0);
    });
   
};

function generateSessionKey() {
    return crypto.randomBytes(16).toString('base64');
}

module.exports = {
    hashAndSaltPassword,
    verifyPassword,
    generateSessionKey
}

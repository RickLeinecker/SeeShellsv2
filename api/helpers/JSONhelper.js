function getOSIndexinArray(array, key) {
    return new Promise(function (resolve, reject) {
        for (var i in array) {
            var item = array[i];
            if (item.os == key) {
                resolve(i);
                return;
            }
        }
        reject(-1);
    });
}

function buildOSFileJSON(databaseResults) {
    return new Promise(async function (resolve, reject) {
        var result = [];
        var os = "";

        for (var i in databaseResults) {
            var osfile = databaseResults[i];
            if (osfile.osname == os) {
                var value = await getOSIndexinArray(result, osfile.osname);
                result[value].files.push(osfile.location);
            }
            else {
                result.push({ "os": osfile.osname, "keysID": osfile.mainkeysid, "files": [osfile.location] });
                os = osfile.osname;
            }
        }

        resolve(result);
    });
}

function getKeyIDIndexinArray(array, key) {
    return new Promise(function (resolve, reject) {
        for (var i in array) {
            var item = array[i];
            if (item.mainkeysid == key) {
                resolve(i);
                return;
            }
        }
        reject(-1);
    });
}

function buildFileLocationJSON(databaseResults) {
    return new Promise(async function (resolve, reject) {
        var result = [];
        var keygroup = "";

        for (var i in databaseResults) {
            var keys = databaseResults[i];
            if (keys.mainkeysid == keygroup) {
                var value = await getKeyIDIndexinArray(result, keys.mainkeysid);
                result[value].keys.push(keys.location);
            }
            else {
                result.push({ "mainkeysid": keys.mainkeysid, "keys": [keys.location] });
                keygroup = keys.mainkeysid;
            }
        }

        resolve(result);
    });
}

module.exports = {
    buildOSFileJSON,
    buildFileLocationJSON
}
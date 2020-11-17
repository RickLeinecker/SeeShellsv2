<template>
    <div id="current">
        <div id="container">
            <div id="leftContent">Enter shell item identifier:</div>
            <div id="middleContent"><b-form-input v-model="identifier" type="number" @change="onChange" placeholder="113"></b-form-input></div>
            <div id="rightContent">Hex equivalent: {{hexIdentifier}}</div>
        </div>
        <br />
        <b-form-textarea id="currentScript"
                         placeholder="Enter Lua script here to parse the shell item..."
                         v-model="text"
                         rows="10"></b-form-textarea>
        <br />
        <b-button @click="onSubmit" variant="primary">{{buttonText}}</b-button>
        <br />
        <b-alert v-model="showSuccessAlert" variant="success" dismissible>
            <strong>Script saved! </strong>To use it in the desktop application, just update your script configuration file in the application.
        </b-alert>
        <b-alert v-model="showErrorAlert" variant="danger" dismissible>
            <strong>Error! </strong> Failed to update the script.
        </b-alert>
    </div>
</template>

<script>

    export default {
        name: 'CurrentScript',
        data() {
            return {
                identifier: '',
                hexIdentifier: '0x71',
                text: '',
                buttonText: 'Select a shell item identifier above.',
                showSuccessAlert: false,
                showErrorAlert: false
            }
        },

        methods: {
            onChange() {
                this.hexIdentifier = '0x' + (Number(this.identifier).toString(16)).toUpperCase();
                if (this.identifier != 0) {
                    // get the existing script if it exists
                    var url = this.$baseurl + 'getScript';
                    var params = 'identifier=' + this.identifier;

                    var xhr = new XMLHttpRequest();
                    xhr.open("GET", url + '?' + params, false);
                    xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");

                    try {
                        xhr.send(null);
                        var result = JSON.parse(xhr.responseText);

                        if (result.success == 1) {
                            this.text = atob(result.script); // decode base64 string
                            this.buttonText = 'Update script';
                        }
                        else {
                            this.text = '';
                            this.buttonText = 'Submit new script';
                        }
                       
                        
                    }
                    catch (err) { 
                        console.info(err);
                    }
                }
            },
            onSubmit() {
                if (this.text == '')
                    return;

                var url = this.$baseurl + 'updateScript';

                var jsonPayload = {identifier: this.identifier, script: btoa(this.text)};

                var xhr = new XMLHttpRequest();
                xhr.open("POST", url, false);
                xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");
                xhr.setRequestHeader("X-Auth-Token", this.$session.get('session'));

                try {
                    xhr.send(JSON.stringify(jsonPayload));
                    var result = JSON.parse(xhr.responseText);

                    if (result.success == 1) {
                        this.showSuccessAlert = true;
                    }
                    else if (result.message == "You must log in to perform this action.") {
                            this.$session.destroy();
                            this.$router.push('/SeeShells/login');
                            location.reload();
                    }
                    else {
                        this.showErrorAlert = true;
                    }
                }
                catch (err) { 
                    console.log(err);
                    this.showErrorAlert = true;
                }
            }
        }
    }
</script>

<style scoped>
    #container {
        height: 100%; 
        width:100%; 
        font-size: 0;

    }
    #leftContent, #middleContent, #rightContent {
        display: inline-block; 
        *display: inline; 
        vertical-align: middle;
        font-size: 25px;

    }
    #leftContent {
        width: 50%; 

    }
    #middleContent {
        width: 25%; 

    }
    #rightContent {
        width: 25%;
        font-size: 15px;
    }
</style>
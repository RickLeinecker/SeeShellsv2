<template>
    <div id="guidHeader">
        <div id="container">
            <div id="leftContent"><b-form-input v-model="guid" type="text" placeholder="GUID"></b-form-input></div>
            <div id="middleContent"><b-form-input v-model="name" type="text" placeholder="Name"></b-form-input></div>
            <div id="rightContent"><b-button @click="onSubmit" variant="primary">Submit name for GUID</b-button></div>
        </div>
        <br />

        <p>If this GUID already exists, it will be overwritten with the name you input here.</p>

        <b-alert v-model="showSuccessAlert" variant="success" dismissible>
            <strong>GUID saved! </strong>To use it in the desktop application, just update your GUID configuration file in the application.
        </b-alert>
        <b-alert v-model="showErrorAlert" variant="danger" dismissible>
            <strong>Error! </strong> Failed to add or update the GUID.
        </b-alert>

    </div>
</template>

<script>

    export default {
        name: 'NewGUID',
        data() {
            return {
                guid: '',
                name: '',
                showSuccessAlert: false,
                showErrorAlert: false
            }
        },
        methods: {
            onSubmit() {
                if (this.guid == '' || this.name == '')
                    return;

                var url = this.$baseurl + 'addGUID';

                var jsonPayload = { guid: this.guid, name: this.name };

                var xhr = new XMLHttpRequest();
                xhr.open("POST", url, false);
                xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");
                xhr.setRequestHeader("X-Auth-Token", this.$session.get('session'));

                try {
                    xhr.send(JSON.stringify(jsonPayload));
                    var result = JSON.parse(xhr.responseText);

                    if (result.success == 1) {
                        location.reload();
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
        width: 35%; 

    }
    #middleContent {
        width: 35%; 

    }
    #rightContent {
        width: 30%;
        font-size: 15px;
    }
    
    p {
        text-align: center;
    }
</style>
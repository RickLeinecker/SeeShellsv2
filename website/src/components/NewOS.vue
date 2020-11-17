<template>
    <div id="container">
        <br />
        <div>
            <div id="leftContent"><b-form-input v-model="versionnumber" type="number" step="0.1" min="10.1" placeholder="OS Version (6.3)"></b-form-input></div>
            <div id="rightContent"><b-form-input v-model="name" type="text" placeholder="OS Name (Windows 8.1)"></b-form-input></div>
        </div>
        <br />
        <b-form-group label="Select where the shellbag data is stored:">
            <b-form-select v-model="keygroupID"
                           :options="keysList"
                           required>
                <template v-slot:first>
                    <b-form-select-option :value="null" disabled>-- Please select an option --</b-form-select-option>
                    <b-form-select-option :value="-1">None of the below apply</b-form-select-option>
                </template>
            </b-form-select>

            <table v-if="keygroupID!=-1" id="keyTable" class="table table-bordered" style="width:100%">
                <thead>
                    <tr>
                        <th>Key Group Number</th>
                        <th>Shellbag file locations</th>
                    </tr>
                </thead>

                <tbody>
                    <!-- table generated here -->
                    <tr v-for="item in keysList" :key="item.value">
                        <td width="25%">{{item.text}}</td>
                        <td>{{item.keys}}</td>
                    </tr>
                </tbody>

            </table>

            <b-form-textarea v-else id="newFiles"
                             placeholder="Enter the shellbag locations here. Separate locations with a new line."
                             v-model="locationtext"
                             rows="4"></b-form-textarea>
        </b-form-group>

        <b-button @click="onSubmit" variant="primary">Submit new OS</b-button>

        <b-alert v-model="showSuccessAlert" variant="success" dismissible>
            <strong>OS and files saved! </strong>To use it in the desktop application, just update your OS configuration file in the application.
        </b-alert>
        <b-alert v-model="showErrorAlert" variant="danger" dismissible>
            <strong>Error! </strong> Failed to add the registry keys or the OS version.
        </b-alert>

    </div>
</template>

<script>

    export default {
        name: 'NewOS',
        data() {
            return {
                versionnumber: '',
                name: '',
                keygroupID: null,
                keysList: [{ text: 'Select One', value: null }],
                locationtext: '',
                showSuccessAlert: false,
                showErrorAlert: false
            }
        },
        methods: {
            populateForm() {
                this.keysList = [];
                var url = this.$baseurl + 'getRegistryLocations';

                var xhr = new XMLHttpRequest();
                xhr.open("GET", url, false);
                xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");

                try {
                    xhr.send(null);
                    var result = JSON.parse(xhr.responseText);

                    if (result.success == 1) {
                        var arr = Array.from(result.json);

                        for (var i = 0; i < arr.length; i++) {
                            this.keysList.push({ value: arr[i].mainkeysid, text: 'Group ' + (i + 1), keys: arr[i].keys.join(', ') });
                        }
                    }

                }
                catch (err) {
                    console.info(err);
                }
            },
            onSubmit() {
                if (this.versionnumber == '' || this.name == '' || this.keygroupID == null)
                    return;

                if (this.keygroupID == -1 && this.locationtext == '')
                    return;

                var url;
                var jsonPayload;

                if (this.keygroupID == -1) { // add a new OS with new registry key locations
                    var keys = this.locationtext.split('\n');

                    url = this.$baseurl + 'addOSWithFileLocations';
                    jsonPayload = {osnum: this.versionnumber, osname: this.name, keys: keys};
                }
                else { // add a new OS that uses the same registry keys as previous OS versions
                    url = this.$baseurl + 'addOS';
                    jsonPayload = {osnum: this.versionnumber, osname: this.name, mainkeysid: this.keygroupID }; 
                }

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
        },   
        mounted() {
            this.$nextTick().then(this.populateForm);
        }
    }
</script>

<style scoped>
    #container {
        height: 100%;
        width: 80%;
        margin: auto;
    }

    #leftContent, #rightContent {
        display: inline-block;
        *display: inline;
        vertical-align: middle;
        font-size: 25px;
    }

    #leftContent {
        width: 50%;
    }

    #rightContent {
        width: 50%;
    }

    p {
        text-align: center;
    }
</style>
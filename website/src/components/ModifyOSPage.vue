<template>
    <div id="osHeader">
        <h1>Add or Remove ShellBag Locations From OS Versions</h1>
        <div id="container">
            <br />
            <b-form-group label="Select the OS version you would like to modify:">
                <b-form-select v-model="osIndex"
                               :options="osList"
                               required @change="onChange">
                    <template v-slot:first>
                        <b-form-select-option :value="null" disabled>-- Please select an option --</b-form-select-option>
                    </template>
                </b-form-select>

            </b-form-group>
            <div>
                <div id="leftContent"><b-form-input v-model="newlocation" type="text" placeholder="Location (NTUSER.DAT\Software\Microsoft\Windows\Shell\BagMRU)" /></div>
                <div id="rightContent"><b-button @click="onSubmit" variant="primary">Submit New Location</b-button></div>
            </div>
            <br />

            <table v-if="keysList.length != 0" id="keyTable" class="table table-bordered" style="width:100%">
                <thead>
                    <tr>
                        <th>Shellbag file locations</th>
                        <th>Delete</th>
                    </tr>
                </thead>

                <tbody>
                    <!-- table generated here -->
                    <tr v-for="item in keysList" :key="item.value">
                        <td>{{item.location}}</td>
                        <td>
                            <a class='btn btn-sm btn-primary' style='color:white;width:50px;' v-on:click="deleteKey(item.location, item.mainkeysid)">X</a>
                        </td>
                    </tr>
                </tbody>
            </table>

        </div>
        <br />
        <div id="osContent">
            <ViewOSandFiles />
        </div>
    </div>
</template>

<script>

    import ViewOSandFiles from './ViewOSandFiles.vue';
    import CheckIfAuthenticated from '../mixins/CheckIfAuthenticated';

    export default {
        name: 'ModifyOSPage',
        components: { ViewOSandFiles },
        mixins: [CheckIfAuthenticated],
        data() {
            return {
                newlocation: '',
                osIndex: null,
                osList: [],
                keysList: []
            }
        },
        methods: {
            populateForm() {
                this.osList = [];
                this.keysList = [];
                this.osIndex = null;
                this.newlocation = '';

                var url = this.$baseurl + 'getOSandRegistryLocations';

                var xhr = new XMLHttpRequest();
                xhr.open("GET", url, false);
                xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");

                try {
                    xhr.send(null);
                    var result = JSON.parse(xhr.responseText);

                    if (result.success == 1) {
                        var arr = Array.from(result.json);

                        for (var i = 0; i < arr.length; i++) {
                            this.osList.push({ text: arr[i].os, value: i, mainkeysid: arr[i].keysID, files: arr[i].files });
                        }
                    }

                }
                catch (err) {
                    console.info(err);
                }
            },
            onChange() {
                this.keysList = [];

                var files = this.osList[this.osIndex].files;
                var mainkeys = this.osList[this.osIndex].mainkeysid;
                 for (var i = 0; i < files.length; i++) {
                    this.keysList.push({ location: files[i], mainkeysid: mainkeys });
                 }
            },
            onSubmit() {
                if (this.osIndex == null)
                    return;

                var url = this.$baseurl + 'addKey';

                var jsonPayload = { osname: this.osList[this.osIndex].text, mainkeysid: this.osList[this.osIndex].mainkeysid, keyToAdd: this.newlocation};

                var xhr = new XMLHttpRequest();
                xhr.open("POST", url, false);
                xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");
                xhr.setRequestHeader("X-Auth-Token", this.$session.get('session'));

                try {
                    xhr.send(JSON.stringify(jsonPayload));
                    var result = JSON.parse(xhr.responseText);

                    if (result.success == 1) {
                        this.populateForm();
                    }
                    else if (result.message == "You must log in to perform this action.") {
                        this.$session.destroy();
                        this.$router.push('/SeeShells/login');
                        location.reload();
                    }
                }
                catch (err) {
                    console.log(err);
                }
            },
            deleteKey(location, keysid) {
                if(confirm("Do you really want to delete this OS version?")){

                    var url = this.$baseurl + 'deleteKey';

                    var jsonPayload = { osname: this.osList[this.osIndex].text, mainkeysid: keysid, keyToDelete: location};

                    var xhr = new XMLHttpRequest();
                    xhr.open("POST", url, false);
                    xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");
                    xhr.setRequestHeader("X-Auth-Token", this.$session.get('session'));

                    try {
                        xhr.send(JSON.stringify(jsonPayload));
                        var result = JSON.parse(xhr.responseText);

                        if (result.success == 1) {
                            this.populateForm();
                        }
                        else if (result.message == "You must log in to perform this action.") {
                            this.$session.destroy();
                            this.$router.push('/SeeShells/login');
                            location.reload();
                        }
                    }
                    catch (err) {
                        console.log(err);
                    }
                }
            }
        },
        mounted() {
            this.$nextTick().then(this.populateForm);
        }
    }
</script>

<style scoped>
    #osHeader {
        margin: 50px;
    }

    #osContent {
        margin: auto;
        height: 100%;
        width: 70%;
    }

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
        width: 70%;
    }

    #rightContent {
        width: 30%;
    }

    p {
        text-align: center;
    }
</style>
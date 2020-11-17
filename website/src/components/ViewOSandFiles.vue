<template>
    <div id="content-wrapper">
        <h2>Current OS Versions Supported:</h2>
        <div class="container-fluid">

            <!-- Page Content -->
            <h3 v-if="osList.length==0">No Windows OS to view!</h3>
            <table v-else id="osTable" class="table table-bordered" style="width:100%">
                <thead>
                    <tr>
                        <th>OS name</th>
                        <th>Shellbag file locations</th>
                        <th>Delete</th>
                    </tr>
                </thead>

                <tbody>
                    <!-- table generated here -->
                    <tr v-for="item in osList" :key="item.os">
                        <td width="25%">{{item.name}}</td>
                        <td>{{item.fileString}}</td>
                        <td>
                            <a class='btn btn-sm btn-primary' style='color:white;width:50px;' v-on:click="deleteOS(item.name, item.mainkeysid)">X</a>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<!-- DataTables -->
<script src="vendor/datatables/jquery.dataTables.js"></script>
<script src="vendor/datatables/dataTables.bootstrap4.js"></script>
<script>
    export default {
        name: "ViewOSandFiles",
        data() {
            return {
                osList: []
            }
        },
        methods: {
            populateTable() {
                this.osList = [];
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
                            this.osList.push({ name: arr[i].os, mainkeysid: arr[i].keysID, files: arr[i].files, fileString: arr[i].files.join(', ') });
                        }
                    }

                }
                catch (err) {
                    console.info(err);
                }

            },
            deleteOS(name, keysid) {

                if(confirm("Do you really want to delete this OS version?")){

                    var url = this.$baseurl + 'deleteOS';

                    var jsonPayload = { osname: name, mainkeysid: keysid};

                    var xhr = new XMLHttpRequest();
                    xhr.open("POST", url, false);
                    xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");
                    xhr.setRequestHeader("X-Auth-Token", this.$session.get('session'));

                    try {
                        xhr.send(JSON.stringify(jsonPayload));
                        var result = JSON.parse(xhr.responseText);

                        if (result.success == 1) {
                            this.populateTable();
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
            this.$nextTick().then(this.populateTable);
        }
    }
</script>
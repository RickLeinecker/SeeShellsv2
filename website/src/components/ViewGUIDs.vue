<template>
    <div id="content-wrapper">
        <h2>Current GUIDs:</h2>
        <div class="container-fluid">

            <!-- Page Content -->
            <h3 v-if="guidList.length==0">No GUIDS to view!</h3>
            <table v-else id="guidTable" class="table table-bordered" style="width:100%">
                <thead>
                    <tr>
                        <th>GUID</th>
                        <th>Name</th>
                        <th>Delete</th>
                    </tr>
                </thead>

                <tbody>
                    <!-- table generated here -->
                    <tr v-for="item in guidList" :key="item.id">
                        <td width="55%">{{item.GUID}}</td>
                        <td>{{item.name}}</td>
                        <td>
                            <a class='btn btn-sm btn-primary' style='color:white;width:50px;' v-on:click="deleteGUID(item.id)">X</a>
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
        name: "GUIDList",
        data() {
            return {
                guidList: []
            }
        },
        methods: {
            populateTable() {
                this.guidList = [];
                var url = this.$baseurl + 'getGUIDs';

                var xhr = new XMLHttpRequest();
                xhr.open("GET", url, false);
                xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");

                try {
                    xhr.send(null);
                    var result = JSON.parse(xhr.responseText);

                    if (result.success == 1) {
                        var arr = Array.from(result.json);

                        for (var i = 0; i < arr.length; i++) {
                            this.guidList.push({ id: arr[i].id, GUID: arr[i].guid, name: arr[i].name });
                        }
                    }

                }
                catch (err) {
                    console.info(err);
                }

            },
            deleteGUID(guidID) {

                if(confirm("Do you really want to delete?")){

                    var url = this.$baseurl + 'deleteGUID';

                    var jsonPayload = { id: guidID };

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
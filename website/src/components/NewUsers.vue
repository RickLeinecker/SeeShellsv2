<template>
    <div id="content-wrapper">

        <div class="container-fluid">

            <!-- Page Content -->
            <h3 v-if="userList.length==0">No new users to approve!</h3>
            <table v-else id="users" class="table table-bordered" style="width:100%">
                <thead>
                    <tr>
                        <th>Username</th>
                        <th>Approve</th>
                        <th>Reject</th>
                    </tr>
                </thead>

                <tbody>
                    <!-- table generated here -->
                    <tr v-for="item in userList" :key="item.id">
                        <td>{{item.username}}</td>
                        <td>
                            <a class='btn btn-sm btn-primary' style='color:white;width:50px;' v-on:click="approveUser(item.id)">O</a>
                        </td>
                        <td>
                            <a class='btn btn-sm btn-primary' style='color:white;width:50px;' v-on:click="rejectUser(item.id)">X</a>
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
        name: "NewUsers",
        data() {
            return {
                userList: []
            }
        },
        methods: {
            populateTable() {
                this.userList = [];
                var url = this.$baseurl + 'getNewUsers';

                var xhr = new XMLHttpRequest();
                xhr.open("GET", url, false);
                xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");

                try {
                    xhr.send(null);
                    var result = JSON.parse(xhr.responseText);

                    if (result.success == 1) {
                        var arr = Array.from(result.json);

                        for (var i = 0; i < arr.length; i++) {
                            this.userList.push({ id: arr[i].id, username: arr[i].username });
                        }
                    }

                }
                catch (err) {
                    console.info(err);
                }
            },

            approveUser(usrID) {
                var url = this.$baseurl + 'approveUser';

                var jsonPayload = { userID: usrID };

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
            },

            rejectUser(usrID) {
                var url = this.$baseurl + 'rejectUser';

                var jsonPayload = { userID: usrID };

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
        },
        mounted() {
            this.$nextTick().then(this.populateTable);
        }
    }
</script>
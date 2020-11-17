<template>
    <div id='items'>
        <b-dropdown-item><router-link to='/SeeShells/approveusers'>Approve New Admins</router-link></b-dropdown-item>
        <b-dropdown-item><router-link to='/SeeShells/guids'>Modify GUIDs</router-link></b-dropdown-item>
        <b-dropdown-item><router-link to='/SeeShells/scripts'>Modify Scripts</router-link></b-dropdown-item>
        <b-dropdown-item><router-link to='/SeeShells/os'>Add New OS</router-link></b-dropdown-item>
        <b-dropdown-item><router-link to='/SeeShells/modifyos'>Modify OS</router-link></b-dropdown-item>
        <b-dropdown-item><router-link to='/SeeShells/updatehelp'>Update Help Page</router-link></b-dropdown-item>
        <b-dropdown-item v-on:click="Logout">Logout</b-dropdown-item>
    </div>
</template>

<script>

    export default {
        name: 'LoggedIn',
        methods: {
            Logout() {
                var url = this.$baseurl + 'logout';
                var xhr = new XMLHttpRequest();
                xhr.open("GET", url, false);
                xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");
                xhr.setRequestHeader("X-Auth-Token", this.$session.get('session'));

                try {
                    xhr.send();
                }
                catch (err) { 
                    console.log(err.message)
                }

                this.$session.destroy();
                this.$router.push('/SeeShells/');
                location.reload();
            }
        }
    }
</script>
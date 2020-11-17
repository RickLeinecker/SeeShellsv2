<template>
    <div id="login">
        <b-form class="m-5" @submit="onLogin">
            <b-form-group label="Username:">
                <b-form-input v-model="form.name" required placeholder="Enter username"></b-form-input>
            </b-form-group>

            <b-form-group label="Password:">
                <b-form-input v-model="form.password" type="password" required placeholder="Enter password"></b-form-input>

                <b-alert v-model="showErrorAlert" variant="danger" dismissible>
                    <strong>Error! </strong> Failed to login. Please try again.
                </b-alert>

            </b-form-group>

            <b-button type="login" variant="primary">Login</b-button>
        </b-form>
    </div>
</template>

<script>
    export default {
        name: 'LoginForm',
        data() {
          return {
            form: {
                  name: '',
                  password: ''
            },
            showErrorAlert: false
          }
        },
        methods: {
            onLogin(event) {
                event.preventDefault();
                var url = this.$baseurl + 'login';

                var jsonPayload = { username: this.form.name, password: this.form.password };

                var xhr = new XMLHttpRequest();
                xhr.open("POST", url, false);
                xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");

                try {
                    xhr.send(JSON.stringify(jsonPayload));
                    var result = JSON.parse(xhr.responseText);

                    if (result.success == 1) {
                        this.$session.start();
                        this.$session.set('session', result.session);
                        this.$router.push('/SeeShells/');
                        location.reload();
                    }
                    else {
                        this.showErrorAlert = true;
                    }
                }
                catch (err) { 
                    this.showErrorAlert = true;
                }
            }
        }
    }
</script>

<style>
    #login {
        margin: auto;
        margin-top:100px;
        height: 100%;
        width: 40%;
    }
</style>
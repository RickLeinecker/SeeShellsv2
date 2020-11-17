<template>
  <div id="register">
    <b-form class="m-5" @submit="onRegister">
      <b-form-group label="Username:" >
        <b-form-input v-model="form.name" required placeholder="Enter username"></b-form-input>
      </b-form-group>

      <b-form-group label="Password:">
          <b-form-input v-model="form.password" type="password" required placeholder="Enter password"></b-form-input>
          <password v-model="form.password" :strength-meter-only="true" />
          <b-form-input v-model="form.passwordconfirm" type="password" required placeholder="Re-enter password"></b-form-input>

          <b-alert v-model="showSuccessAlert" variant="success" dismissible>
              <strong>Registration sent! </strong>You must wait for a current administrator to approve you now.
          </b-alert>
          <b-alert v-model="showErrorAlert" variant="danger" dismissible>
              <strong>Error! </strong> {{errorMessage}}
          </b-alert>

      </b-form-group>

      <b-button type="register" variant="primary">Register</b-button>
    </b-form>
  </div>
</template>

<script>
    import Password from 'vue-password-strength-meter'
    export default {
        name: 'RegisterForm',
        components: { Password },
        data() {
            return {
                form: {
                    name: '',
                    password: '',
                    passwordconfirm: ''
                },
                showSuccessAlert: false,
                showErrorAlert: false,
                errorMessage: ''
            }
        },
        methods: {
            onRegister(event) {
                event.preventDefault();            

                if (this.form.password == this.form.passwordconfirm) {
                    var jsonPayload = { username: this.form.name, password: this.form.password };
                    var url = this.$baseurl + 'register';

                    var xhr = new XMLHttpRequest();
                    xhr.open("POST", url, false);
                    xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");

                    try {
                        xhr.send(JSON.stringify(jsonPayload));
                        var result = JSON.parse(xhr.responseText);

                        if (result.success == 1) {
                            this.showSuccessAlert = true;
                        }
                        else {
                            this.errorMessage = result.error + ' Please try again.';
                            this.showErrorAlert = true;
                        }

                    }
                    catch (err) { 
                        console.log(err.message);
                        this.errorMessage = 'Failed to register account.';
                        this.showErrorAlert = true;
                    }
                }
                else {
                    this.errorMessage = 'Passwords don\'t match! Re-enter the passwords.';
                    this.showErrorAlert = true;
                }
            }
        }
    }
</script>

<style>
    #register{
        margin: auto;
        margin-top:100px;
        height: 100%;
        width: 40%;
    }
</style>
import React from 'react';
import beach from '../assets/beach2.png';
import { withStyles } from '@material-ui/core/styles';
import { withRouter } from 'react-router-dom';
import TextField from '@material-ui/core/TextField';
import Button from '@material-ui/core/Button';

const styles = {
    image: {
        display: 'flex',
        position: 'relative',
        width: '100%',
        height: '600px',
        textAlign: 'center',
        justifyContent: 'center',
        backgroundImage: 'url(' + beach + ')',
    },
    loginContainer: {
        display: 'flex',
        justifyContent: 'center',
    },
    login: {
        display: 'flex',
        justifyContent: 'center',
        flexDirection: 'column',
        backgroundColor: '#424242',
        width: '50%',
        alignItems: 'center',
        color: 'white',
        fontSize: '40px',
    },
    field: {
        display: 'flex',
        flexDirection: 'column',
        color: 'white',
        fontSize: '20px',
        width: '80%',
    },
    button: {
        backgroundColor: '#1D70EB',
        color: 'white',
    },
    input: {
        backgroundColor: 'white',
    }
}

class AdminLogin extends React.Component {
    constructor(props) {
        super(props);

        this.handleEmail = this.handleEmail.bind(this);
        this.handlePassword = this.handlePassword.bind(this);
        this.login = this.login.bind(this);
    }
    
    handleEmail(event) {
        this.setState({ email: event.target.value });
    }

    handlePassword(event) {
        this.setState({ password: event.target.value });
    }

    login() {
        const email = this.state.email;
        const password = this.state.password;

        var xhr = new XMLHttpRequest();
        xhr.addEventListener('load', () => {
            console.log(xhr.responseText)
        });
        xhr.open('POST', );
        xhr.send(JSON.stringify({ email, password }));
    }

    render() {
        return(
            <div className={this.props.classes.loginContainer}>
                <div className={this.props.classes.image}/>
                <div className={this.props.classes.login}>
                    Admin Login
                    <div className={this.props.classes.field}>
                        Email:
                        <TextField variant="filled" color="#1D70EB" className={this.props.classes.input} onChange={this.handleEmail}/>
                    </div>
                    <div className={this.props.classes.field}>
                        Password:
                        <TextField variant="filled" color="#1D70EB" className={this.props.classes.input} onChange={this.handlePassword} type="password"/>
                    </div>
                    <Button className={this.props.classes.button} onClick={this.login}>Login</Button>
                </div>
            </div>
        );
    }
}

export default withStyles(styles)(withRouter(AdminLogin));
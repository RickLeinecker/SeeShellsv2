import React from 'react';
import beach from '../../assets/beach2.png';
import { withStyles } from '@material-ui/core/styles';
import { withRouter } from 'react-router-dom';
import TextField from '@material-ui/core/TextField';
import Button from '@material-ui/core/Button';

const styles = {
    image: {
        display: 'flex',
        width: '100%',
        height: '600px',
        textAlign: 'center',
        justifyContent: 'center',
        alignSelf: 'center',
        backgroundImage: 'url(' + beach + ')',
    },
    loginContainer: {
        display: 'flex',
        justifyContent: 'center',
        width: '100%',
    },
    login: {
        display: 'flex',
        justifyContent: 'center',
        flexDirection: 'column',
        backgroundColor: '#424242',
        width: '30%',
        alignItems: 'center',
        color: 'white',
        fontSize: '40px',
        height: '70%',
        position: 'absolute',
        alignSelf: 'center',
    },
    loginTitle: {
        fontSize: '50px',
        fontWeight: 'bold',
    },
    field: {
        display: 'flex',
        flexDirection: 'column',
        color: 'white',
        fontSize: '20px',
        width: '80%',
        padding: '3%',
    },
    button: {
        display: 'flex',
        backgroundColor: '#33A1FD',
        '&:hover': {
            backgroundColor: '#EF476F',
        },
        color: 'white',
        fontSize: '20px',
        margin: '0px',
        justifyContent: 'center',
        marginTop: '5%',
    },
    register: {
        display: 'flex',
        backgroundColor: '#EF476F',
        '&:hover': {
            backgroundColor: '#33A1FD',
        },
        color: 'white',
        fontSize: '20px',
        margin: '0px',
        justifyContent: 'center',
        marginTop: '5%',
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
        this.register = this.register.bind(this);
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
        xhr.open('POST', this.props.history.pathname);
        xhr.send(JSON.stringify({ email, password }));
    }

    register(event) {
        this.props.history.push("/" + event.currentTarget.id);
    }

    render() {
        return(
            <div className={this.props.classes.loginContainer}>
                <div className={this.props.classes.image}/>
                <div className={this.props.classes.login}>
                    <p className={this.props.classes.loginTitle}>Administrative Login</p>
                    <div className={this.props.classes.field}>
                        Email:
                        <TextField variant="filled" color="#1D70EB" className={this.props.classes.input} onChange={this.handleEmail}/>
                    </div>
                    <div className={this.props.classes.field}>
                        Password:
                        <TextField variant="filled" color="#1D70EB" className={this.props.classes.input} onChange={this.handlePassword} type="password"/>
                    </div>
                    <Button className={this.props.classes.button} onClick={this.login}>Login</Button>
                    <Button className={this.props.classes.register} onClick={this.register} id="register">Not registered? Sign up</Button>
                </div>
            </div>
        );
    }
}

export default withStyles(styles)(withRouter(AdminLogin));
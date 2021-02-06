import React from 'react';
import oldLogo from '../assets/oldLogo.png';
import { withStyles } from '@material-ui/core/styles';
import { withRouter, BrowserRouter as Router } from 'react-router-dom';
import Button from '@material-ui/core/Button';

const styles = {
    root: {
        backgroundColor: '#424242',
        width: '100%',
        display: 'flex',
        margin: '0px',
    },
    buttonContainer: {
        display: 'flex',
        float: 'right',
        marginLeft: 'auto',
    },
    buttons: {
        display: 'flex',
        justifyContent: 'center',
        color: 'white',
        fontSize: '20px',
        fontFamily: 'Georgia',
    },
    title: {
        fontFamily: 'Georgia',
        fontSize: '50px',
        margin: '10px',
        display: 'flex',
        paddingLeft: '10px',
        color: 'white',
    },
    logo: {
        height: '50px',
        display: 'flex',
        paddingTop: '15px',
        paddingLeft: '10px',
        float: 'left',
    },
};

class MenuBar extends React.Component {
    constructor(props) {
        super(props);

        this.handleClick = this.handleClick.bind(this);
    }

    handleClick(event) {
        this.props.history.push("/" + event.currentTarget.id);
    }

    render() {
        return(
            <Router>
                <div className={this.props.classes.root}>
                    <img src={oldLogo} alt='SeeShells Logo' className={this.props.classes.logo} onClick={this.handleClick}/>
                    <p className={this.props.classes.title}>SEESHELLS</p>
                    <div className={this.props.classes.buttonContainer}> 
                        <Button className={this.props.classes.buttons} onClick={this.handleClick} id="about">About</Button>
                        <Button className={this.props.classes.buttons} onClick={this.handleClick} id="download">Download</Button>
                        <Button className={this.props.classes.buttons} onClick={this.handleClick} id="documentation">Documentation</Button>
                        <Button className={this.props.classes.buttons} onClick={this.handleClick} id="developers">Developers</Button>
                        <Button className={this.props.classes.buttons} onClick={this.handleClick} id="login">Admin Login</Button>
                    </div>
                </div>
            </Router>
        );
    }
}

export default withStyles(styles)(withRouter(MenuBar));
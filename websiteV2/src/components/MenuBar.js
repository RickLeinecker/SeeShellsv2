import React from 'react';
import oldLogo from '../assets/oldLogo.png';
import { withStyles } from '@material-ui/core/styles';
import Button from '@material-ui/core/Button';

const styles = {
    root: {
        backgroundColor: '#424242',
        width: '100%',
        display: 'flex',
    },
    buttonContainer: {
        display: 'flex',
        justifyContent: 'flex-end',
    },
    buttons: {
        display: 'flex',
        justifyContent: 'center',
        color: 'white',
    },
    title: {
        fontFamily: 'Georgia',
        fontSize: '50px',
        margin: '10px',
        display: 'flex',
        paddingLeft: '10px',
    },
    logo: {
        height: '50px',
        display: 'flex',
        paddingTop: '15px',
        paddingLeft: '10px',
        float: 'left',
    },
};

// TODO: the buttons need to float right
class MenuBar extends React.Component {
    render() {
        return(
            <div className={this.props.classes.root}>
                <img src={oldLogo} alt='SeeShells Logo' className={this.props.classes.logo}/>
                <p className={this.props.classes.title}>SEESHELLS</p>
                <div className={this.props.classes.buttonContainer}> 
                    <Button className={this.props.classes.buttons}>About</Button>
                    <Button className={this.props.classes.buttons}>Download</Button>
                    <Button className={this.props.classes.buttons}>Documentation</Button>
                    <Button className={this.props.classes.buttons}>Developers</Button>
                    <Button className={this.props.classes.buttons}>Admin Login</Button>
                </div>
            </div>
        );
    }
}

export default withStyles(styles)(MenuBar);
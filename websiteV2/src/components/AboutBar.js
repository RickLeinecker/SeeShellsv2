import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { withRouter } from 'react-router-dom';
import Paper from '@material-ui/core/Paper';
import Button from '@material-ui/core/Button';
import ButtonGroup from '@material-ui/core/ButtonGroup';

const styles = {
    container: {
        height: '100%',
        width: '100%',
    },
    sidebarContainer: {
        width: '100%',
        height: '100%',
        backgroundColor: '#424242',
        margin: '0px',
        display: 'flex',
        flexDirection: 'column',
        overflow: 'auto',
        borderRadius: '0',
        minWidth: '300px',
    },
    primaryButtons: {
        display: 'flex',
        justifyContent: 'left',
        color: '#F2F2F2',
        '&:hover': {
            color: '#33A1FD',
        },
        fontSize: '30px',
        fontFamily: 'Georgia',
    },
    buttons: {
        display: 'flex',
        justifyContent: 'center',
        color: '#F2F2F2',
        '&:hover': {
            color: '#33A1FD',
        },
        fontSize: '20px',
        fontFamily: 'Georgia',
    },
};

/*
*   AboutBar.js
*   - component that controls the navigation on all /about pages
*   - new pages can be added by creating a new button, replacing the id and text fields, and adding the id to the App.js routes
*/
class AboutBar extends React.Component {
    constructor(props) {
        super(props);

        this.handleClick = this.handleClick.bind(this);
    }

    /*
    *   handleClick(event)
    *   - takes in a react event object on every button click
    *   - redirects the user to the page associated with the button's id
    */
    handleClick(event) {
        if (event.currentTarget.id === "about") {
            this.props.history.push("/");
        } else {
            this.props.history.push("/" + event.currentTarget.id);
        }
    }

    render() {
        return(
            <div className={this.props.classes.container}>
                <Paper elevation={2} square={true} className={this.props.classes.sidebarContainer}>
                    <Button className={this.props.classes.primaryButtons} onClick={this.handleClick} id="about">About</Button>
                    <ButtonGroup orientation="vertical">
                        <Button className={this.props.classes.buttons} onClick={this.handleClick} id="registry">Windows Registry</Button>
                        <Button className={this.props.classes.buttons} onClick={this.handleClick} id="shellbags">Shellbags</Button>
                    </ButtonGroup>
                    <Button className={this.props.classes.buttons} onClick={this.handleClick} id="parser">SeeShells Parsing</Button>
                    <Button className={this.props.classes.buttons} onClick={this.handleClick} id="analysis">SeeShells Analysis</Button>
                    <Button className={this.props.classes.primaryButtons} onClick={this.handleClick} id="case-studies">Case Studies</Button>
                </Paper>
            </div>
        );
    }
}

export default withStyles(styles)(withRouter(AboutBar));
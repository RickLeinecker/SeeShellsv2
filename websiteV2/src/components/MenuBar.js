import React from 'react';
import logo from '../assets/seeshellsLogo-100.png';
import logoClicked from '../assets/seeshellsLogo-100-click.png';
import { withStyles } from '@material-ui/core/styles';
import { withRouter, HashRouter as Router } from 'react-router-dom';
import { Button, AppBar, Toolbar, Paper, Collapse, Tooltip } from '@material-ui/core';
import DehazeIcon from '@material-ui/icons/Dehaze';

const styles = {
    menuBar: {
        backgroundColor: '#212121',
        width: '100%',
        display: 'flex',
        margin: '0px',
        overflow: 'auto',
    },
    buttonContainer: {
        display: 'flex',
        float: 'right',
        marginLeft: 'auto',
        marginRight: '1%',
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
    title: {
        fontFamily: 'Georgia',
        fontSize: 'calc(20px + 2vw)',
        margin: '10px',
        display: 'flex',
        paddingLeft: '10px',
        color: '#F2F2F2',
    },
    logo: {
        height: '70px',
        width: '70px',
        display: 'flex',
        float: 'left',
        backgroundImage: 'url(' + logo + ')',
        backgroundSize: 'cover',
        '&:hover': {
            backgroundImage: 'url(' + logoClicked + ')',
        },
    },
    dropdownContainer: {
        backgroundColor: '#212121',
        width: '100%',
        display: 'flex',
        justifyContent: 'center',
        flexDirection: 'space-evenly',
        flexFlow: 'wrap',
    },
};

/*
*   MenuBar.js
*   - handles all top bar sitewide navigation
*/
class MenuBar extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            hideNav: false,
            dropdown: false
        };

        this.handleClick = this.handleClick.bind(this);
        this.toggleMenu = this.toggleMenu.bind(this);
    }

    componentDidMount() {
        window.addEventListener("resize", this.resize.bind(this));
        this.resize();
    }
    
    /*
    *   resize()
    *   - along with componentDidMount() and componentWillUnmount(), this function determines whether or not the user is in mobile view
    */
    resize() {
        this.setState({hideNav: window.innerWidth <= 760});
    }
    
    componentWillUnmount() {
        window.removeEventListener("resize", this.resize.bind(this));
    }

    /*
    *   handleClick(event)
    *   - takes in a react event object on every button click
    *   - redirects the user to the page associated with the button's id
    *   - closes the dropdown menu after a new page is selected
    */
    handleClick(event) {
        this.props.history.push("/" + event.currentTarget.id);
        this.setState({ dropdown: false });
    }

    /*
    *   toggleMenu()
    *   - opens or closes the mobile view menu
    *   - this.state.dropdown === true: mobile view menu is open
    *   - this.state.dropdown === false: mobile view menu is closed
    */
    toggleMenu() {
        this.setState({ dropdown: !this.state.dropdown });
    }

    render() {
        return(
            <Router basename="/">
                <AppBar position="static" className={this.props.classes.menuBar}>
                    <Toolbar>
                        <Tooltip title='Return Home'>
                            <div alt='SeeShells Logo' className={this.props.classes.logo} onClick={this.handleClick}/>
                        </Tooltip>
                        <p className={this.props.classes.title}>SEESHELLS</p>
                        {!this.state.hideNav && 
                            <div className={this.props.classes.buttonContainer}> 
                                <Button className={this.props.classes.buttons} onClick={this.handleClick} id="about">About</Button>
                                <Button className={this.props.classes.buttons} onClick={this.handleClick} id="download">Download</Button>
                                <Button className={this.props.classes.buttons} onClick={this.handleClick} id="documentation">Documentation</Button>
                                <Button className={this.props.classes.buttons} onClick={this.handleClick} id="developers">Developers</Button>
                            </div>
                        }
                        {this.state.hideNav &&
                            <div> 
                                <Button className={this.props.classes.buttons} onClick={this.toggleMenu}><DehazeIcon/></Button>
                            </div>
                        }
                    </Toolbar>
                    {this.state.hideNav &&
                        <Collapse in={this.state.dropdown}>
                            <Paper className={this.props.classes.dropdownContainer}>
                                <Button className={this.props.classes.buttons} onClick={this.handleClick} id="about">About</Button>
                                <Button className={this.props.classes.buttons} onClick={this.handleClick} id="download">Download</Button>
                                <Button className={this.props.classes.buttons} onClick={this.handleClick} id="documentation">Documentation</Button>
                                <Button className={this.props.classes.buttons} onClick={this.handleClick} id="developers">Developers</Button>
                            </Paper>
                        </Collapse>
                    }
                </AppBar>
            </Router>
        );
    }
}

export default withStyles(styles)(withRouter(MenuBar));
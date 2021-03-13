import React from 'react';
import oldLogo from '../assets/oldLogo.png';
import { withStyles } from '@material-ui/core/styles';
import { withRouter, HashRouter as Router } from 'react-router-dom';
import Button from '@material-ui/core/Button';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Paper from '@material-ui/core/Paper';
import Collapse from '@material-ui/core/Collapse';
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
        fontSize: '50px',
        margin: '10px',
        display: 'flex',
        paddingLeft: '10px',
        color: '#F2F2F2',
    },
    logo: {
        height: '50px',
        width: '50px',
        display: 'flex',
        float: 'left',
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

class MenuBar extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            hideNav: false,
            dropdown: false
        };

        this.handleClick = this.handleClick.bind(this);
        this.openMenu = this.openMenu.bind(this);
    }

    componentDidMount() {
        window.addEventListener("resize", this.resize.bind(this));
        this.resize();
    }
    
    resize() {
        this.setState({hideNav: window.innerWidth <= 760});
    }
    
    componentWillUnmount() {
        window.removeEventListener("resize", this.resize.bind(this));
    }

    handleClick(event) {
        this.props.history.push("/" + event.currentTarget.id);
    }

    openMenu() {
        this.setState({ dropdown: !this.state.dropdown });
    }

    render() {
        return(
            <Router basename="/">
                <AppBar position="static" className={this.props.classes.menuBar}>
                    <Toolbar>
                        <img src={oldLogo} alt='SeeShells Logo' className={this.props.classes.logo} onClick={this.handleClick}/>
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
                            <div className={this.props.classes.buttonContainer}> 
                                <Button className={this.props.classes.buttons} onClick={this.openMenu}><DehazeIcon/></Button>
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
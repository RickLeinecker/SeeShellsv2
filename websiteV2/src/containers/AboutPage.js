import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { withRouter, HashRouter as Router } from 'react-router-dom';
import AboutBar from '../components/AboutBar.js';
import WindowsRegistry from '../components/about-page/WindowsRegistry.js';
import Shellbags from '../components/about-page/Shellbags.js';
import Parsing from '../components/about-page/Parsing.js';
import Analysis from '../components/about-page/Analysis.js';
import CaseStudies from '../components/about-page/CaseStudies.js';
import { Paper, Typography, Collapse, Button } from '@material-ui/core';
import ArrowDropDownIcon from '@material-ui/icons/ArrowDropDown';
import Welcome from '../assets/seeshells-welcome.png';

const styles = {
    content: {
        height: '100%',
        width: '100%',
        display: 'flex',
        justifyContent: 'flex-start',
        flexDirection: 'column',
        alignItems: 'center',
        overflow: 'auto',
        borderRadius: '0',
    },
    title: {
        fontSize: '50px',
        fontWeight: 'bold',
        marginTop: '1%',
        alignSelf: 'center',
        color: '#33A1FD',
        textAlign: 'center',
    },
    text: {
        textAlign: 'center',
        padding: '1%',
    },
    sidebarContainer: {
        height: '100%',
        display: 'flex',
        alignContent: 'center',
        flexDirection: 'column',
        borderRadius: '0',
    },
    toggle: {
        backgroundColor: '#424242',
        borderRadius: '0',
    },
    topBar: {
        width: '100%',
        display: 'flex',
        justifyContent: 'center',
        backgroundColor: '#424242',
        alignContent: 'center',
        flexDirection: 'column',
        borderRadius: '0',
    },
    buttons: {
        color: 'white',
    }
};

/*
*   AboutPage.js
*   - handles all /about pages and renders content depending on what version of the about page is open
*/
class AboutPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            showBar: true,
        };

        this.toggleDropdown = this.toggleDropdown.bind(this);
    }

    /*
    *   toggleDropdown()
    *   - opens or closes the mobile view menu
    *   - this.state.dropdown === true: mobile view menu is open
    *   - this.state.dropdown === false: mobile view menu is closed
    */
    toggleDropdown() {
        this.setState({ dropdown: !this.state.dropdown });
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

    render() {
        return(
            <Router basename="/about">
                {!this.state.hideNav &&
                    <Paper className={this.props.classes.sidebarContainer}>
                        <AboutBar/>
                    </Paper>
                }
                <Paper elevation={0} className={this.props.classes.content}>
                    {this.state.hideNav &&
                        <Paper className={this.props.classes.topBar}>
                            <Button className={this.props.classes.buttons} onClick={this.toggleDropdown}><ArrowDropDownIcon/></Button>
                            <Collapse in={this.state.dropdown}>
                                <Paper>
                                    <AboutBar/>
                                </Paper>
                            </Collapse>
                        </Paper>
                    }
                    {this.props.subpage === "about" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>About</Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                SeeShells is a digital forensics tool that parses and analyzes shellbag data from the Windows 
                                Registry, and displays the data in a graphical interface. The graphical interface can be 
                                interacted with and filters can be applied to find shellbag data in a quick and easy-to-use manner.
                            </Typography>
                            <img src={Welcome} alt="seeshells-welcome" />
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                Select a subheading to learn more about the Windows Registry, shellbags, and SeeShells.
                            </Typography>
                        </Paper>
                    }
                    {this.props.subpage === "registry" &&
                        <Paper className={this.props.classes.content}>
                            <WindowsRegistry/>
                        </Paper>
                    }
                    {this.props.subpage === "shellbags" &&
                        <Paper className={this.props.classes.content}>
                            <Shellbags/>
                        </Paper>
                    }
                    {this.props.subpage === "parser" &&
                        <Paper className={this.props.classes.content}>
                            <Parsing/>
                        </Paper>
                    }
                    {this.props.subpage === "analysis" &&
                        <Paper className={this.props.classes.content}>
                            <Analysis/>
                        </Paper>
                    }
                    {this.props.subpage === "case-studies" &&
                        <Paper className={this.props.classes.content}>
                            <CaseStudies/>
                        </Paper>
                    }
                </Paper>
            </Router>
        );
    }
}

export default withStyles(styles)(withRouter(AboutPage));
import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { withRouter, HashRouter as Router } from 'react-router-dom';
import AboutBar from '../components/AboutBar.js';
import WindowsRegistry from '../components/about-page/WindowsRegistry.js';
import Shellbags from '../components/about-page/Shellbags.js';
import SeeShells from '../components/about-page/SeeShells.js';
import Parsing from '../components/about-page/Parsing.js';
import Analysis from '../components/about-page/Analysis.js';
import Timeline from '../components/about-page/Timeline.js';
import CaseStudies from '../components/about-page/CaseStudies.js';
import { Paper, Typography, Collapse, Button } from '@material-ui/core';
import ArrowDropDownIcon from '@material-ui/icons/ArrowDropDown';
import ReactPlayer from "react-player";

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
    video: {
        maxHeight: '360px',
        maxWidth: '640px',
        minHeight: '150px',
        minWidth: '300px',
        height: '50%',
        width: '50%',
        margin: '10px',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        backgroundColor: '#424242',
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
                                SeeShells is an information extraction software. SeeShells is a standalone, 
                                open source executable that can run both online and offline registry hives. 
                                It extracts and parses through Windows Registry information. This data is then 
                                converted into two forms. The first is a csv file that contains all the raw data 
                                we obtain. The second is a human readable timeline. The timeline provides an 
                                interactive, easier to read visualization of the data extracted from the windows registries. 
                                This information is otherwise difficult and time consuming to comb through and understand. 
                                The application is a great way to gain insight into what someone has done on their computer over time. 
                                The program can be particularly useful for digital forensics investigators as the information 
                                can be downloaded and used as evidence in a court of law. 
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                Windows uses the Shellbag keys to store user preferences for GUI folder display within Windows Explorer. 
                                Everything from visible columns to display mode (icons, details, list, etc.) to sort order are tracked. 
                                If you have ever made changes to a folder and returned to that folder to find your new preferences intact, 
                                then you have seen Shellbags in action. 
                            </Typography>
                            <Paper className={this.props.classes.video}>
                                <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=IZrd86723Hc"/>
                            </Paper>
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
                    {this.props.subpage === "seeshells" &&
                        <Paper className={this.props.classes.content}>
                            <SeeShells/>
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
                    {this.props.subpage === "timeline" &&
                        <Paper className={this.props.classes.content}>
                            <Timeline/>
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
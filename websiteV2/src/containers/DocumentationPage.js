import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { withRouter, HashRouter as Router } from 'react-router-dom';
import { Paper, Typography, Collapse, Button } from '@material-ui/core';
import ArrowDropDownIcon from '@material-ui/icons/ArrowDropDown';

import DocumentationBar from '../components/DocumentationBar.js';
import OnlineParsing from '../components/documentation-page/OnlineParsing.js';
import OfflineParsing from '../components/documentation-page/OfflineParsing.js';
import ShellInspector from '../components/documentation-page/ShellInspector.js';
import EventTimeline from '../components/documentation-page/EventTimeline.js';
import ShellbagEvents from '../components/documentation-page/ShellbagEvents.js';
import ShellbagTable from '../components/documentation-page/ShellbagTable.js';
import HexViewer from '../components/documentation-page/HexViewer.js';
import RegistryView from '../components/documentation-page/RegistryView.js';
import Filtering from '../components/documentation-page/Filtering.js';
import Exporting from '../components/documentation-page/Exporting.js';
import Hints from '../components/documentation-page/Hints.js';
import Themes from '../components/documentation-page/Themes.js';
import Reset from '../components/documentation-page/Reset.js';

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
};

/*
*   DocumentationPage.js
*   - handles all /documentation pages and renders content depending on what version of the documentation page is open
*/
class DocumentationPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            showBar: true,
            dropdown: false,
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

    // TODO: split up all the informational page content for ease of updating
    // TODO: add images (convert all to webp?)
    // TODO: make the docs more reader-friendly
    render() {
        return(
            <Router basename="/documentation">
                {!this.state.hideNav &&
                    <Paper className={this.props.classes.sidebarContainer}>
                        <DocumentationBar/>
                    </Paper>
                }
                <Paper elevation={0} className={this.props.classes.content}>
                    {this.state.hideNav &&
                        <Paper className={this.props.classes.topBar}>
                            <Button className={this.props.classes.buttons} onClick={this.toggleDropdown}><ArrowDropDownIcon/></Button>
                            <Collapse in={this.state.dropdown}>
                                <Paper>
                                    <DocumentationBar/>
                                </Paper>
                            </Collapse>
                        </Paper>
                    }
                    {this.props.subpage === "documentation" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>How To Use</Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                <b>Select a subheading for step-by-step guides</b> on how to use the SeeShells application.
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                SeeShells is intended to be run on Windows machines. 
                            </Typography>
                            <Paper className={this.props.classes.video}>
                                <ReactPlayer height='100%' width='100%' url="https://youtu.be/fzK-bUQrIxg?t=194"/>
                            </Paper>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                To view the full case study, please visit our <a href="#/about/case-studies">case study page</a>.
                            </Typography>
                        </Paper>
                    }
                    {this.props.subpage === "online" &&
                        <Paper className={this.props.classes.content}>
                            <OnlineParsing/>
                        </Paper>
                    }
                    {this.props.subpage === "offline" &&
                        <Paper className={this.props.classes.content}>
                            <OfflineParsing/>
                        </Paper>
                    }
                    {this.props.subpage === "inspector" &&
                        <Paper className={this.props.classes.content}>
                            <ShellInspector/>
                        </Paper>
                    }
                    {this.props.subpage === "timeline" &&
                        <Paper className={this.props.classes.content}>
                            <EventTimeline/>
                        </Paper>
                    }
                    {this.props.subpage === "events" &&
                        <Paper className={this.props.classes.content}>
                            <ShellbagEvents/>
                        </Paper>
                    }
                    {this.props.subpage === "table" &&
                        <Paper className={this.props.classes.content}>
                            <ShellbagTable/>
                        </Paper>
                    }
                    {this.props.subpage === "hex" &&
                        <Paper className={this.props.classes.content}>
                            <HexViewer/>
                        </Paper>
                    }
                    {this.props.subpage === "registry" &&
                        <Paper className={this.props.classes.content}>
                            <RegistryView/>
                        </Paper>
                    }
                    {this.props.subpage === "filters" &&
                        <Paper className={this.props.classes.content}>
                            <Filtering/>
                        </Paper>
                    }
                    {this.props.subpage === "export" &&
                        <Paper className={this.props.classes.content}>
                            <Exporting/>
                        </Paper>
                    }
                    {this.props.subpage === "hints" &&
                        <Paper className={this.props.classes.content}>
                            <Hints/>
                        </Paper>
                    }
                    {this.props.subpage === "themes" &&
                        <Paper className={this.props.classes.content}>
                            <Themes/>
                        </Paper>
                    }
                    {this.props.subpage === "reset" &&
                        <Paper className={this.props.classes.content}>
                            <Reset/>
                        </Paper>
                    }
                </Paper>
            </Router>
        );
    }
}

export default withStyles(styles)(withRouter(DocumentationPage));
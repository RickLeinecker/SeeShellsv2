import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { withRouter, HashRouter as Router } from 'react-router-dom';
import Paper from '@material-ui/core/Paper';
import DocumentationBar from '../components/DocumentationBar.js';
import Typography from '@material-ui/core/Typography';
import Slide from '@material-ui/core/Slide';
import Collapse from '@material-ui/core/Collapse';
import Button from '@material-ui/core/Button';
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
                                SeeShells collects ShellBags specific Windows Registry keys and parses through them, 
                                and organizes the data found in them to display them on a graphical timeline. The 
                                graphical timeline is the unique feature that SeeShells offers over other existing parsers: 
                                this timeline makes ShellBag data easier to understand and facilitates the process of
                                finding a significant pattern or piece of evidence.
                            </Typography>
                        </Paper>
                    }
                    {this.props.subpage === "online" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Online Parsing</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "offline" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Offline Parsing</Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                To get an offline hive for SeeShells from a Windows machine:
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                1. Access the Windows File Explorer as an Admin user
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                2. Navigate to $USERHOME\AppData\Local\Microsoft\Windows\
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>[image placeholder]</Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                3. Copy and Paste the UsrClass.dat file from the Windows folder to the destination folder
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                Currently, SeeShells only supports the parsing of .dat files. UsrClass.dat is the 
                                recommended file to be parsed in SeeShells since it contains information pertaining to 
                                shellbags. Conversely, NTUSER.dat is a supported filetype, but contains no information 
                                pertaining to shellbags and will not be usful to a SeeShells user.
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                To load a registry hive into SeeShells:
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                1. Run SeeShells
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                2. Under "Import Registry" on the Start Menu, select "From Registry File"
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>[image placeholder]</Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                a. Alternatively, a user can navigate to the global menu in the upper left corner and select
                                 File > Import > From Offline Registry
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>[image placeholder]</Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                3. In the File Dialog, navigate to the UsrClass.dat file to be parsed
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                4. Click on the UsrClass.dat file and select Open
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>[image placeholder]</Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                5. The Timeline, Registry, Shellbags, and Events views will populate with the parsed file contents
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>[image placeholder]</Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                Once the UsrClass.dat file is loaded into SeeShells, the user will be able to explore the 
                                shellbags and extrapolated shellbag events using the multiple views available in SeeShells.
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>[image placeholder]</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "advanced" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Advanced Configuration</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "toolbar" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Toolbar</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "data" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Viewing The Data</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "timeline" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Timeline View</Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                The Timeline view allows users to view ShellBag Events in chronological order.
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                The Timeline view starts at the lowest zoom level, providing users with a chronologically 
                                sorted activity histogram that allows the user to quickly identify time periods of high 
                                and low levels of activity on the machine.
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>[image placeholder]</Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                The user can narrow the timespan being viewed by using the mouse scroll wheel:
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                1. While hovering the cursor over the area they want to expand, the user can move the 
                                mouse scroll wheel up to zoom in and shorten the viewable timespan and enlarge the cards.
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                2. As the user zooms in, the Shellbag event cards will sort themselves so that the 
                                viewable cards fall within the timespan area focused on by the user.
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                3. By zooming in further, the user can further narrow the timespan and Shellbag 
                                Event data will begin to populate the cards.
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                4. At this zoom level, the user can either further zoom in or navigate the timeline using the listed controls:
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                a. To navigate left and right on the Timeline, the user can click and drag the Timeline left or right to navigate in either direction
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                i. Alternatively, holding the ALT key will allow the user to scroll left and right on the Timeline using the mouse scroll wheel
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                b. To navigate up and down on the Timeline, the user can click and drag the Timeline up or down to navigate in either direction
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                i. Alternatively, holding the CTRL key will allow the user to scroll up and down on the Timeline using the mouse scroll wheel
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>[image placeholder]</Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                5. To zoom out, the user can move the mouse wheel down and this will lengthen the viewable timespan 
                                and shrink the cards, allowing the user to view the activity histogram.
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                6. Alternatively, users can filter the Shellbag Events on the Timeline using the Begin Date and End 
                                Date filters on the Filter Controls panel:
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                a. Setting a Begin Date will filter the list for all Shellbag Events occurring after that date
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                b. Setting an End Date will filter the list for all Shellbag Events occurring before that date
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>[image placeholder]</Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                7. Using the Filter Controls, the user will be able to shorten the displayed timespan without needing to use the mouse scroll wheel controls.
                            </Typography>
                        </Paper>
                    }
                    {this.props.subpage === "events" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Shellbag Events</Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                Shellbags store information that can be used to make inferences about how a Windows machine was used in the past. 
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>[image placeholder]</Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                SeeShells analyzes the shellbags stored in a Windows registry hive and interprets them as Shell Events. 
                                Each Shell Event documents an action taken by a user that resulted in a change to their Windows Machine. 
                                For example, when a user connects a removable storage device to their Windows system, the file system is 
                                altered to display the removable storage device and its contents. The resulting changes to the user's 
                                Windows registry hive can be read and interpreted by SeeShells as a Shell Event that describes the Removable 
                                Storage Device Connect action.
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                Shell Events contain key information about an action taken on a Windows system including the type of 
                                action, the user that performed the action, the time at which the action was taken, any file system 
                                locations associated with the action, and all parsed shellbag data that provide evidence for the action.
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                Shellbag Events include:
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                Item Creation
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                Item Last Access
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                Item Last Modify
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                Item Last Registry Write
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                Program Installation Event
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                Feature Update Event
                            </Typography>
                            <Typography variant="subtitle1" className={this.props.classes.text}>
                                File Download Event 
                            </Typography>
                        </Paper>
                    }
                    {this.props.subpage === "filters" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Shellbag Filtering</Typography>
                        </Paper>
                    }
                    {this.props.subpage === "export" &&
                        <Paper className={this.props.classes.content}>
                            <Typography variant="title" className={this.props.classes.title}>Exporting</Typography>
                        </Paper>
                    }
                </Paper>
            </Router>
        );
    }
}

export default withStyles(styles)(withRouter(DocumentationPage));
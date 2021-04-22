import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { Typography, Card } from '@material-ui/core';
import PersonPinIcon from '@material-ui/icons/PersonPin';
import SeeShellsUI from '../../assets/live-registry-parse.png';

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
    text_alt: {
        padding: '1%',
    },
    quote: {
        color: 'white',
        backgroundColor: '#424242',
        padding: '2%',
        maxWidth: '500px',
        maxHeight: '300px',
        minWidth: '500px',
        minHeight: '300px',
        margin: '1%',
        display: 'flex',
        flexDirection: 'column',
        position: 'relative',
    },
    icon: {
        width: '75px',
        height: '75px',
        alignSelf: 'flex-end',
        paddingTop: '5%',
        position: 'absolute',
        bottom: '5px',
    },
}

class Analysis extends React.Component {

    render() {
        return (
            <div className={this.props.classes.content}>
                <Typography variant="title" className={this.props.classes.title}>SeeShells Shellbag Analysis</Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    SeeShells analyzes the parsed shellbags by finding patterns in the shell items 
                    and categorizing them into shellbag events. 
                </Typography>
                <Card className={this.props.classes.quote}>
                    <Typography variant="button" className={this.props.classes.text_alt}>
                        <b>Quote from the devs:</b>
                    </Typography>
                    <Typography variant="subtitle2" className={this.props.classes.text}>
                        ❝ The intermediate timeline events from the 
                        timestamp extraction phase are analyzed for patterns that correspond to specific 
                        user actions. For example, an event may be generated 
                        because a shellbag timestamp was found that documents the exact time that 
                        a directory was created in a user’s Program Files (x86) folder. In this case, 
                        the intermediate event created from that timestamp was consumed to create a 
                        “Program Installation Event”.  ❞
                    </Typography>
                    <PersonPinIcon className={this.props.classes.icon}/>
                </Card>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    In addition, SeeShells has a graphical user interface that displays frequency 
                    analysis of these shellbags and shellbag events. 
                    The graphical interface consists of six main features, the Event Timeline view, the 
                    Shell Inspector, the Hex Viewer, the Registry View, the Global Event Filters, and the 
                    Raw Shellbag Table. All of these views work together to give the user a thorough 
                    analysis of shellbag items. Furthermore, this data can be exported into a report.
                </Typography>
                <img src={SeeShellsUI} alt='seeshells-ui'/>
            </div>
        )
    }
}

export default withStyles(styles)(Analysis);
import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
import ShellInspector from '../../assets/shell-inspector.png';

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
}

class OnlineParsing extends React.Component {
    
render() {
        return(
            <div className={this.props.classes.content}>
                <Typography variant="title" className={this.props.classes.title}>Shell Inspector</Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    The shell inspector displays the raw shellbag data fields. 
                </Typography>
                <img src={ShellInspector} alt="shellbag-inspector" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    The data fields most common to all shellbag types are displayed at the top, while more specific and varied data is contained within
                    the Shellbag Information and Shellbag Fields. Shellbags can have different fields depending on shellbag types.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    After selecting a shellbag from one of the other views, the user can use this display to look at the data fields.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    This information is extracted from an offset in the hex values, visible in the Hex Viewer.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    When shellbag events have occurred roughly within a minute of each other, multiple shellbags can appear 
                    in this view and the user can scroll through each of them. This typically happens with Windows updates.
                </Typography>
            </div>
            
        )
    }
}

export default withStyles(styles)(OnlineParsing);
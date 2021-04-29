import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { Typography, Paper } from '@material-ui/core';
import ReactPlayer from "react-player";
import TimelineFilter from '../../assets/timeline-filter.png';
import TimelineColor from '../../assets/timeline-color.png';
import Tooltips from '../../assets/tooltip.png';
import TimelineView from '../../assets/timeline.png';

const styles = {
    content: {
        height: '100%',
        width: '100%',
        display: 'flex',
        justifyContent: 'flex-start',
        flexDirection: 'column',
        alignItems: 'center',
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
        minHeight: '360px',
        minWidth: '300px',
        height: '50%',
        width: '50%',
        margin: '10px',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        backgroundColor: '#424242',
    },
}


class Timeline extends React.Component {

    render() {
        return(
            <div className={this.props.classes.content}>
                <Paper className={this.props.classes.video}>
                    <ReactPlayer height='100%' width='100%' url="https://www.youtube.com/watch?v=Cea0mb0ONsU"/>
                </Paper>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    The timeline contains a color-coded overview of shellbag events. 
                </Typography>
                <img src={TimelineView} alt="timeline" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Events can be filtered on the timeline by selecting the colored square next to the shellbag event labels.
                </Typography>
                <img src={TimelineFilter} alt="timeline-filtering" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    A user can also select methods of coloring the timeline to get the frequency of certain actions on the registry.
                </Typography>
                <img src={TimelineColor} alt="timeline-color-options" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    <b>To navigate the timeline:</b>
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    1. To easily narrow down a section of time, hold down CTRL, right click on the timeline, and drag the mouse in either direction. 
                    A box will appear, allowing the user to highlight a section of interest, and immediately zoom into that section of time.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    2. Alternatively, the user can use the mouse scroll wheel to zoom in and out. Holding down CTRL while scrolling on the mouse wheel will
                    slow down the zoom speed.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    3. The user can move the timeline right and left by holding down right click and dragging the mouse in the either direction.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    The user can also open up tooltips about particular sections on the timeline by left clicking on a block of color.
                </Typography>
                <img src={Tooltips} alt='timeline-tooltip'/>
            </div>
        );
    }
}

export default withStyles(styles)(Timeline);
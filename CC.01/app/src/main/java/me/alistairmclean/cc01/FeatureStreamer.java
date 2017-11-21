package me.alistairmclean.cc01;
import android.util.Log;

import java.io.DataOutputStream;
import java.io.IOException;
import java.net.Socket;
import java.net.UnknownHostException;


public class FeatureStreamer implements Runnable{
    private String mHOST_IP;
    private int mHOST_PORT;
    private static Socket mSocket;
    private static DataOutputStream mDOS;
    private static boolean mIsConnected = false;



    FeatureStreamer(String addr, int port) {
        if(addr != null)
            mHOST_IP = addr;
        if(mHOST_IP != null)
            mHOST_PORT = port;

    }

    void connect(String addr, int port) {
        Log.d("FEATURE STREAMER: ", "add = " + addr + " port = " + port);
        try {
            mSocket = new Socket(addr, port);
            mDOS = new DataOutputStream(mSocket.getOutputStream());
            mIsConnected = true;
        } catch (UnknownHostException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    void sendByteArray(byte[] send) throws IOException {
        if (mDOS != null) {
            mDOS.writeInt(send.length);
            mDOS.write(send, 0, send.length);
            mDOS.flush();
        }
    }
    boolean isConnected() {
        return mIsConnected;
    }

    void sendFeatures(int width, int height, byte[] send) {
        try {
            if (mDOS != null) {
                mDOS.writeInt(width);
                mDOS.writeInt(height);
                mDOS.write(send, 0, send.length);
                mDOS.flush();
            }
        } catch (IOException e) {
        }
    }

    void close() throws IOException {
        if (mDOS != null) {
            mDOS.close();
        }
        if (mSocket != null) {
            mSocket.close();
        }
        mIsConnected = false;
    }
    public void sendFeaturesToServer(byte[] rgb, byte[] yuv420sp,
                                         int width, int height) {
        Thread thread1 = new Thread(new DecodeAndSend(rgb,yuv420sp,width,height));
        thread1.start();
    }



    @Override
    public void run() {
        connect(mHOST_IP, mHOST_PORT);
        //TODO - figure out how to send the data properly to the server.

    }

    //Decodes and sends to the server
    static class DecodeAndSend implements Runnable {
        private static byte[] RGB;
        private static byte[] YUV420SP;
        private static int WIDTH;
        private static int HEIGHT;

        public DecodeAndSend(byte[] rgb, byte[] yuv420sp,
                       int width, int height){
            RGB = new byte[rgb.length];
            RGB = rgb;
            YUV420SP = new byte[yuv420sp.length];
            YUV420SP = yuv420sp;
            WIDTH = width;
            HEIGHT = height;
        }
        private static void decodeYUV420SPGrayscale(byte[] rgb, byte[] yuv420sp,
                                                   int width, int height) {
            final int frameSize = width * height;

            for (int pix = 0; pix < frameSize; pix++) {
                int pixVal = (0xff & ((int) yuv420sp[pix])) - 16;
                if (pixVal < 0)
                    pixVal = 0;
                if (pixVal > 255)
                    pixVal = 255;
                rgb[pix] = (byte) pixVal;
            }
        }


        @Override
        public void run() {
            decodeYUV420SPGrayscale(RGB,YUV420SP,WIDTH, HEIGHT);
            //Start the
            Thread sendingThread = new Thread(new SendFeaturesToServer(WIDTH,HEIGHT, YUV420SP));
            sendingThread.start();
        }
    }

    static class SendFeaturesToServer implements Runnable {
        private static int WIDTH;
        private static int HEIGHT;
        private static byte[] SEND;
        public SendFeaturesToServer(int width, int height, byte[] send){
            WIDTH = width;
            HEIGHT = height;
            SEND = new byte[send.length];
            SEND = send;
        }

        void sendFeatures(int width, int height, byte[] send) {
            try {
                if (mDOS != null) {
                    mDOS.writeInt(width);
                    mDOS.writeInt(height);
                    mDOS.write(send, 0, send.length);
                    mDOS.flush();
                }
            } catch (IOException e) {
            }
        }
        @Override
        public void run() {
            while(mIsConnected) {
                sendFeatures(WIDTH,HEIGHT,SEND);
            }

        }
    }
}
